using FlightMath.DB;
using FlightMath.DTO;
using FlightMath.Models;
using FlightMath.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace FlightMath.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightMathController : ControllerBase
    {
        private readonly ILogger<FlightMathController> _logger;

        private readonly FlightDbContext _dbContext;
        private readonly ParametersParser _parser;
        private readonly Validator _validator;

        private const int EarthRadius = 6373;

        public FlightMathController(ILogger<FlightMathController> logger,
            FlightDbContext dbContext, ParametersParser parser, Validator validator)
        {
            _logger = logger ?? throw new NullReferenceException($"{nameof(logger)} is null");
            _dbContext = dbContext ?? throw new NullReferenceException($"{nameof(dbContext)} is null");
            _parser = parser ?? throw new NullReferenceException($"{nameof(parser)} is null");
            _validator = validator ?? throw new NullReferenceException($"{nameof(validator)} is null");
        }

        /// <summary>
        /// Общая статистика по рейсу
        /// </summary>
        /// <returns>Возвращается объект описывающий общую статистику по рейсу</returns>
        [HttpGet("Statistics")]
        public IEnumerable<Statistics> GetStatistics()
        {
            try
            {
                return _dbContext.GetDataWithAirports.AsParallel().Select(data =>
                {
                    List<Tuple<string, string>> pathSections
                        = _parser.ParseStringElements(data.Origin).Zip(
                            _parser.ParseStringElements(data.Dest),
                            (origin, destination) => new Tuple<string, string>(origin, destination)).ToList();

                    List<double> sectionsDistance = CalculateDistance(data.Airports, pathSections);
                    decimal distance = Convert.ToDecimal(sectionsDistance.Sum());

                    return new Statistics()
                    {
                        Sequence = data.Sequence,
                        Carrier = data.Carrier,
                        Flights = data.Flights,
                        Dates = data.Dates,
                        Origin = data.Origin,
                        Dest = data.Dest,
                        AWBcount = pathSections.Count,
                        Weight = CalculateWeight(data.ActualKGs, pathSections.Count).Sum(),
                        Distance = distance,
                        Revenue = data.PCWeight / distance ?? 0
                    };
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Возникла ошибка в методе {nameof(GetStatistics)}:{Environment.NewLine}{ex}");
                return new List<Statistics>();
            }
        }

        /// <summary>
        /// Рейс разделенный на секции
        /// </summary>
        /// <returns>Возвращается секция рейса</returns>
        [HttpGet("SegmentsInfo")]
        public IEnumerable<SegmentInfo> GetSegmentsInfo()
        {
            try
            {
                return _dbContext.MainData.AsNoTracking().AsParallel().SelectMany(data =>
                {
                    IEnumerable<string> flightsValues = _parser.ParseNumberElements(data.Flights);
                    IEnumerable<string> originValues = _parser.ParseStringElements(data.Origin);
                    IEnumerable<string> destValues = _parser.ParseStringElements(data.Dest);
                    IEnumerable<string> datesValues = _parser.ParseDateElements(data.Dates);
                    IEnumerable<string> carrierValues = _parser.ParseStringAndNumElements(data.Carrier);

                    List<decimal> weightValues = CalculateWeight(data.ActualKGs, flightsValues.Count());

                    List<SegmentInfo> segmentsInfo = new List<SegmentInfo>(flightsValues.Count());

                    foreach (int i in Enumerable.Range(0, flightsValues.Count()))
                    {
                        segmentsInfo.Add(new SegmentInfo()
                        {
                            Sequence = data.Sequence,
                            AWBSeq = data.AWBSeq,
                            Prefix = data.Prefix,
                            Serial = data.Serial,
                            PC_Weight = data.PCWeight,
                            Flights = flightsValues.ElementAt(i),
                            Origin = originValues.ElementAt(i),
                            Dates = datesValues.ElementAt(i),
                            Dest = destValues.ElementAt(i),
                            Carrier = carrierValues.ElementAt(i),
                            ActualKGs = weightValues.ElementAt(i)
                        });
                    }

                    return segmentsInfo;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Возникла ошибка в методе {nameof(GetSegmentsInfo)}:{Environment.NewLine}{ex}");
                return new List<SegmentInfo>();
            }
        }

        [HttpGet("FlightRate")]
        public FlightRate GetFlightRate([FromQuery] string origin, [FromQuery] string destination)
        {
            try
            {
                if (!(_validator.ValidateIATACode(origin) && _validator.ValidateIATACode(destination)))
                    return new FlightRate() { Origin = origin, Destination = destination, Rate = 0 };

                List<MainData> flights = _dbContext.MainData.AsNoTracking()
                    .Where(data => data.Origin.StartsWith(origin)
                                   && data.Dest.EndsWith(destination)).ToList();

                IEnumerable<decimal> rates = flights.SelectMany(flight =>
                {
                    if (flight.PCWeight != null && flight.PCWeight != 0)
                    {
                        return new[]
                        {
                            (flight.PCWeight / CalculateWeight(flight.ActualKGs, _parser.ParseNumberElements(flight.Flights).Count()).Sum()).Value
                        };
                    }
                    else
                        return new decimal[0];
                });

                return new FlightRate()
                {
                    Origin = origin,
                    Destination = destination,
                    Rate = rates.Any() ? rates.Average() : 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Возникла ошибка в методе {nameof(GetFlightRate)}:{Environment.NewLine}{ex}");
                return new FlightRate() { Origin = origin, Destination = destination, Rate = 0 };
            }
        }

        //Подсчет веса
        private List<decimal> CalculateWeight(string weightString, int parametersCount)
        {
            IEnumerable<string> floatWeightValues = _parser.ParseFloatElements(weightString);

            List<decimal> results = new List<decimal>();


            if (floatWeightValues.Count() == parametersCount)
            {
                floatWeightValues.ToList().ForEach(
                    value =>
                    {
                        results.Add(
                            decimal.Parse(value.Trim(',').Replace(',', '.'), CultureInfo.InvariantCulture));
                    });
            }
            else
            {
                IEnumerable<string> weightValues = _parser.ParseNumberElements(weightString);
                if (weightValues.Count() == parametersCount)
                {
                    weightValues.ToList().ForEach(
                        value =>
                            results.Add(decimal.Parse(value.Trim(',').Replace(',', '.'), CultureInfo.InvariantCulture)));
                }
                else
                {
                    throw new ArgumentException($"Невозможно распарсить строку [{weightString}]. Ожидаемое число параметров [{parametersCount}]");
                }
            }

            return results;
        }

        //Формула гаверсинусов (возвращается в киллометрах)
        private List<double> CalculateDistance(List<Airport> pathAirports, List<Tuple<string, string>> pathSections)
        {
            List<double> sectionsDistanse = new List<double>();


            foreach (Tuple<string, string> section in pathSections)
            {
                Airport origin = pathAirports.Single(a => a.IataCode == section.Item1);
                Airport destination = pathAirports.Single(a => a.IataCode == section.Item2);

                double originLat = Convert.ToDouble(origin.Latitude.Value);
                double originLon = Convert.ToDouble(origin.Longitude.Value);

                double destLat = Convert.ToDouble(destination.Latitude.Value);
                double destLon = Convert.ToDouble(destination.Longitude.Value);

                double sin1 = Math.Sin((originLat - destLat) / 2);
                double sin2 = Math.Sin((originLon - destLon) / 2);
                sectionsDistanse.Add(2 * EarthRadius * Math.Asin(Math.Sqrt(sin1 * sin1 + sin2 * sin2 * Math.Cos(originLat) * Math.Cos(destLat))));
            }

            return sectionsDistanse;
        }
    }
}
