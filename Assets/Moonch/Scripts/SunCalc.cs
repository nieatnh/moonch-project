using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// SunCalc implementation wrapped from https://github.com/mourner/suncalc
/// 
/// Copyright (c) 2014, Vladimir Agafonkin
/// All rights reserved.
/// For more information about the project license check https://github.com/mourner/suncalc/blob/master/LICENSE
/// </summary>

public class DateTimeTools {
    public static DateTime FromUnixTime(long unixTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddMilliseconds(unixTime);
    }

    public static long ToUnixTime(DateTime date)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return Convert.ToInt64((date - epoch).TotalMilliseconds);
    }
}

public class SunCalcGenerals
{
    #region shortcuts for easier to read formulas
    public const double PI = Math.PI;

    public static Func<double, double> cos = Math.Cos;
    public static Func<double, double> sin = Math.Sin;
    public static Func<double, double> tan = Math.Tan;
    public static Func<double, double> asin = Math.Asin;
    public static Func<double, double> atan = Math.Atan;
    public static Func<double, double, double> atan2 = Math.Atan2;
    public static Func<double, double> acos = Math.Acos;
    #endregion

    public const double rad = Math.PI / 180;
    
    #region date/time constants and conversions
    public const double dayMs = 1000 * 60 * 60 * 24;
    public const double J1970 = 2440588;
    public const double J2000 = 2451545;

    public static double toJulian(DateTime date) { 
        return DateTimeTools.ToUnixTime(date) / (double)dayMs - 0.5 + J1970; 
    }
    public static DateTime fromJulian(double j)  { 
        return DateTimeTools.FromUnixTime((long)((j + 0.5 - J1970) * dayMs)); 
    }
    public double toDays(DateTime date)   { 
        return SunCalcGenerals.toJulian(date) - J2000;
    }
    #endregion

    #region general calculations for position

    public const double e = rad * 23.4397; // obliquity of the Earth

    public static double rightAscension(double l, double b) { 
        return atan2(sin(l) * cos(e) - tan(b) * sin(e), cos(l)); 
    }
    public static double declination(double l, double b) 
    { 
        return asin(sin(b) * cos(e) + cos(b) * sin(e) * sin(l)); 
    }

    public static double azimuth(double H, double phi, double dec)  { 
        return atan2(sin(H), cos(H) * sin(phi) - tan(dec) * cos(phi)); 
    }

    public static double altitude(double H, double phi, double dec) {
        return asin(sin(phi) * sin(dec) + cos(phi) * cos(dec) * cos(H)); 
    }

    public static double siderealTime(double d, double lw) { 
        return rad * (280.16 + 360.9856235 * d) - lw; 
    }

    public static double astroRefraction(double h) {
        if (h < 0) // the following formula works for positive altitudes only.
            h = 0; // if h = -0.08901179 a div/0 would occur.

        // formula 16.4 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
        // 1.02 / tan(h + 10.26 / (h + 5.10)) h in degrees, result in arc minutes -> converted to rad:
        return 0.0002967f / tan(h + 0.00312536 / (h + 0.08901179));
    }
    #endregion

    
    #region general sun calculations

    public static double solarMeanAnomaly(double d) { 
        return rad * (357.5291 + 0.98560028 * d); 
    }

    public static double eclipticLongitude(double M) {
        double C = rad * (1.9148 * sin(M) + 0.02 * sin(2 * M) + 0.0003 * sin(3 * M)), // equation of center
            P = rad * 102.9372; // perihelion of the Earth
        return M + C + P + PI;
    }

    public static Coordinate sunCoords(double d) {
        double M = solarMeanAnomaly(d),
            L = eclipticLongitude(M);

        return new Coordinate(declination(L, 0), rightAscension(L, 0));
    }

    public class Coordinate {
        public double dec;
        public double ra;
        public Coordinate(double dec, double ra)
        {
            this.dec = dec;
            this.ra = ra;
        }
    }

    #region calculations for sun times
    public const double J0 = 0.0009;

    public static double julianCycle(double d, double lw) { 
        return Math.Round(d - J0 - lw / (2 * PI)); 
    }

    public static double approxTransit(double Ht, double lw, double n) { 
        return J0 + (Ht + lw) / (2 * PI) + n; 
    }
    public static double solarTransitJ(double ds, double M, double L)  { 
        return J2000 + ds + 0.0053 * sin(M) - 0.0069 * sin(2 * L); 
    }

    public static double hourAngle(double h, double phi, double d) { 
        return acos((sin(h) - sin(phi) * sin(d)) / (cos(phi) * cos(d))); 
    }

    // returns set time for the given sun altitude
    public static double getSetJ(double h, double lw, double phi, double dec, double n, double M, double L) {
        double w = hourAngle(h, phi, dec),
            a = approxTransit(w, lw, n);
        return solarTransitJ(a, M, L);
    }
    #endregion

    public static DateTime hoursLater(DateTime date, double h) {
        long unixEpoch = DateTimeTools.ToUnixTime(date);
        double newUnixEpoch = unixEpoch + h * dayMs / 24;
        return DateTimeTools.FromUnixTime((long)newUnixEpoch);
    }
}


public class SunCalcX : SunCalcGenerals { 
    
    public class TimeInformation {
        public double angle;
        public string morningName;
        public string eveningName;

        public TimeInformation(double angle, string morningName, string eveningName) {
            this.angle = angle;
            this.morningName = morningName;
            this.eveningName = eveningName;
        }

    }
    public class Position {
        public double azimuth;
        public double altitude;
        public Position(double azimuth, double altitude) {
            this.azimuth = azimuth;
            this.altitude = altitude;
        }
    }

    // calculates sun position for a given date and latitude/longitude

    public Position getPosition(DateTime date, double lat, double lng) {
        double lw = rad * -lng;
        double phi = rad * lat;
        double d = toDays(date);
        Coordinate c = sunCoords(d);
        double H  = siderealTime(d, lw) - c.ra;

        return new Position(azimuth(H, phi, c.dec), altitude(H, phi, c.dec));
    }

    // sun times configuration (angle, morning name, evening name)
    List<TimeInformation> times = new List<TimeInformation>(){
        new TimeInformation(-0.833, "sunrise", "sunset"),
        new TimeInformation(-0.3, "sunriseEnd", "sunsetStart"),
        new TimeInformation(-6, "dawn", "dusk"),
        new TimeInformation(-12, "nauticalDawn", "nauticalDusk"),
        new TimeInformation(-18, "nightEnd", "night"),
        new TimeInformation(6, "goldenHourEnd", "goldenHour")
    };
    
    // adds a custom time to the times config
    public void addTime(double angle, string riseName, string setName) {
        times.Add(new TimeInformation(angle, riseName, setName));
    }

    public class TimeResult {
        public DateTime solarNoon;
        public DateTime nadir;
        public TimeResult(DateTime solarNoon, DateTime nadir) {
            this.solarNoon = solarNoon;
            this.nadir = nadir;
            this["solarNoon"] = solarNoon;
            this["nadir"] = nadir;
        }
        public Dictionary<string, DateTime> times = new Dictionary<string, DateTime>();
        public DateTime? this[string key]
        {
            set
            {
                if (!this.times.ContainsKey(key))
                    this.times.Add(key, value.Value);
                this.times[key] = value.Value;
            }
            get
            {
                if (times.ContainsKey(key))
                    return times[key];
                return null;
            }
        }
    }

    public TimeResult getTimes(DateTime date, double lat, double lng)
    {

        double lw = rad * -lng,
            phi = rad * lat,
            d = toDays(date),
            n = julianCycle(d, lw),
            ds = approxTransit(0, lw, n),
            M = solarMeanAnomaly(ds),
            L = eclipticLongitude(M),
            dec = declination(L, 0),
            Jnoon = solarTransitJ(ds, M, L), Jset, Jrise;


        TimeResult result = new TimeResult(fromJulian(Jnoon), fromJulian(Jnoon - 0.5));
        for (int i = 0; i < times.Count; i += 1) {
            var time = times[i];
            Jset = getSetJ(time.angle * rad, lw, phi, dec, n, M, L);
            Jrise = Jnoon - (Jset - Jnoon);

            result[time.morningName] = fromJulian(Jrise);
            result[time.eveningName] = fromJulian(Jset);
        }

        return result;
    }
    #endregion

    #region moon calculations, based on http://aa.quae.nl/en/reken/hemelpositie.html formulas
    public class MoonCoordinates {
        public double ra;
        public double dec;
        public double dist;
        public MoonCoordinates(double ra, double dec, double dist) {
            this.ra = ra;
            this.dec = dec;
            this.dist = dist;
        }
    }
    public MoonCoordinates moonCoords(double d) { // geocentric ecliptic coordinates of the moon
        double L = rad * (218.316 + 13.176396 * d), // ecliptic longitude
            M = rad * (134.963 + 13.064993 * d), // mean anomaly
            F = rad * (93.272 + 13.229350 * d),  // mean distance
            l  = L + rad * 6.289 * sin(M), // longitude
            b  = rad * 5.128 * sin(F),     // latitude
            dt = 385001 - 20905 * cos(M);  // distance to the moon in km

        return  new MoonCoordinates(rightAscension(l, b), declination(l, b), dt);
    }
    #endregion

    public class MoonPosition {
        public double azimuth;
        public double altitude;
        public double distance;
        public double parallacticAngle;
        public MoonPosition(double azimuth,double altitude,double distance,double parallacticAngle) {
            this.azimuth = azimuth;
            this.altitude = altitude;
            this.distance = distance;
            this.parallacticAngle = parallacticAngle;
        }
    }

    public MoonPosition getMoonPosition(DateTime date, double lat, double lng) {
        double lw  = rad * -lng, phi = rad * lat,
            d   = toDays(date);

        MoonCoordinates c = moonCoords(d);
        double H = siderealTime(d, lw) - c.ra;
        double h = altitude(H, phi, c.dec);
        // formula 14.1 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
        double pa = atan2(sin(H), tan(phi) * cos(c.dec) - sin(c.dec) * cos(H));

        h = h + astroRefraction(h); // altitude correction for refraction

        return new MoonPosition(azimuth(H, phi, c.dec), h, c.dist, pa);
    }

    public class MoonIllumination {
        double fraction;
        double phase;
        double angle;

        public MoonIllumination(double fraction, double phase, double angle) {
            this.fraction = fraction;
            this.phase = phase;
            this.angle = angle;
        }
    } 

    // calculations for illumination parameters of the moon,
    // based on http://idlastro.gsfc.nasa.gov/ftp/pro/astro/mphase.pro formulas and
    // Chapter 48 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.

    public MoonIllumination getMoonIllumination(DateTime date) {

        var d = toDays(date);
        var s = sunCoords(d);
        var m = moonCoords(d);

        double sdist = 149598000; // distance from Earth to Sun in km

        double phi = acos(sin(s.dec) * sin(m.dec) + cos(s.dec) * cos(m.dec) * cos(s.ra - m.ra));
        double inc = atan2(sdist * sin(phi), m.dist - sdist * cos(phi));
        double angle = atan2(cos(s.dec) * sin(s.ra - m.ra), sin(s.dec) * cos(m.dec) - cos(s.dec) * sin(m.dec) * cos(s.ra - m.ra));

        return new MoonIllumination(
            (1 + cos(inc)) / 2,
            0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI,
            angle
        );
    }

    public class MoonTimes  {
        public DateTime? rise;
        public DateTime? set;
        public enum Position{AlwaysUp, AlwaysDown}
        public Position? position;
        /*
        public MoonTimes(DateTime rise, DateTime set, Position position) {
            this.rise = rise;
            this.set = set;
            this.position = position;
        }*/
    }

    
// calculations for moon rise/set times are based on http://www.stargazing.net/kepler/moonrise.html article

    public MoonTimes getMoonTimes(DateTime date, double lat, double lng, bool inUTC) {
        var t = date;

        //if (inUTC) t.setUTCHours(0, 0, 0, 0);
        //else t.setHours(0, 0, 0, 0);

        double hc = 0.133 * rad,
            h0 = getMoonPosition(t, lat, lng).altitude - hc,
            h1, h2, a, b, xe, d, roots, dx;

        double x1 = 0;
        double x2 = 0;
        double ye = 0;
        double? rise = null;
        double? set = null;

        // go in 2-hour chunks, each time seeing if a 3-point quadratic curve crosses zero (which means rise or set)
        for (int i = 1; i <= 24; i += 2) {
            h1 = getMoonPosition(hoursLater(t, i), lat, lng).altitude - hc;
            h2 = getMoonPosition(hoursLater(t, i + 1), lat, lng).altitude - hc;

            a = (h0 + h2) / 2 - h1;
            b = (h2 - h0) / 2;
            xe = -b / (2 * a);
            ye = (a * xe + b) * xe + h1;
            d = b * b - 4 * a * h1;
            roots = 0;

            if (d >= 0) {
                dx = Math.Sqrt(d) / (Math.Abs(a) * 2);
                x1 = xe - dx;
                x2 = xe + dx;
                if (Math.Abs(x1) <= 1) roots++;
                if (Math.Abs(x2) <= 1) roots++;
                if (x1 < -1) x1 = x2;
            }

            if (roots == 1) {
                if (h0 < 0) rise = i + x1;
                else set = i + x1;

            } else if (roots == 2) {
                rise = i + (ye < 0 ? x2 : x1);
                set = i + (ye < 0 ? x1 : x2);
            }

            if (rise.HasValue && set.HasValue && rise.Value != 0 && set.Value != 0) break;

            h0 = h2;
        }
        
        MoonTimes moonTimes = new MoonTimes();
        if (rise.HasValue && rise!=0) moonTimes.rise = hoursLater(t, rise.Value);
        if (set.HasValue && set!=0) moonTimes.set = hoursLater(t, set.Value);

        if ((!rise.HasValue || rise == 0) && (!set.HasValue || set == 0)) {
            moonTimes.position = ye > 0 ? MoonTimes.Position.AlwaysUp : MoonTimes.Position.AlwaysDown;
        }

        return moonTimes;
    }
}
