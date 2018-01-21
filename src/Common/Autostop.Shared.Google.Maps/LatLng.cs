﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace Google.Maps
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LatLng : Location, IEquatable<LatLng>
    {
        public LatLng()
        {
        }

        /// <summary>
        ///     Create a new latlng instance with the given latitude and longitude coordinates.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public LatLng(decimal latitude, decimal longitude)
        {
            Latitude = Convert.ToDouble(latitude);
            Longitude = Convert.ToDouble(longitude);
        }

        /// <summary>
        ///     Create a new latlng instance with the given latitude and longitude coordinates.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public LatLng(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        ///     Create a new latlng instance with the given latitude and longitude coordinates.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public LatLng(float latitude, float longitude)
        {
            Latitude = Convert.ToDouble(latitude);
            Longitude = Convert.ToDouble(longitude);
        }

        /// <summary>
        ///     Gets or sets the latitude coordinate
        /// </summary>
        [JsonProperty("lat")]
        public double Latitude { get; }

        /// <summary>
        ///     Gets or sets the longitude coordinate
        /// </summary>
        [JsonProperty("lng")]
        public double Longitude { get; }

        public bool Equals(LatLng other)
        {
            if (other == null) return false;

            if (other.Latitude == Latitude && other.Longitude == Longitude)
                return true;

            //else
            return false;
        }

        /// <summary>
        ///     Gets the string representation of the latitude and longitude coordinates.  Default format is "N6" for 6 decimal
        ///     precision.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("N6");
        }

        /// <summary>
        ///     Gets the string representation of the latitude and longitude coordinates.  The format is applies to a
        ///     System.Double, so any format applicable for System.Double will work.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            var sb = new StringBuilder(50); //default to 50 in the internal array.
            sb.Append(Latitude.ToString(format, CultureInfo.InvariantCulture));
            sb.Append(",");
            sb.Append(Longitude.ToString(format, CultureInfo.InvariantCulture));

            return sb.ToString();
        }

        /// <summary>
        ///     Gets the current instance as a URL encoded value.
        /// </summary>
        /// <returns></returns>
        public override string GetAsUrlParameter()
        {
            // This style of formatting will give us 7 decimal places of precision,
            // but if anything can be expressed with less, then it will be.
            // IE: 0.5 rather than 0.5000000
            return ToString("0.#######");
        }

        /// <summary>
        ///     Parses a LatLng from a set of latitude/longitude coordinates
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LatLng Parse(string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            try
            {
                var parts = value.Split(',');

                if (parts.Length != 2) throw new FormatException("Missing data for points.");

                var latitude = double.Parse(parts[0].Trim(), CultureInfo.InvariantCulture);
                var longitude = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);

                var latlng = new LatLng(latitude, longitude);

                return latlng;
            }
            catch (Exception ex)
            {
                throw new FormatException("Failed to parse LatLng.", ex);
            }
        }

        /// <summary>
        ///     Converts the specified string representation of a latlong to its <see cref="LatLng" /> equivalent and returns a
        ///     value that indicates whether the convertion succeeded.
        /// </summary>
        /// <param name="value">A string containing the latlong to convert.</param>
        /// <param name="result">
        ///     When this method returns, it contains the <see cref="LatLng" /> value equivalent to the value
        ///     contained in value, if the convertion succeeded; otherwize null if the convertion failed. The convertion fails if
        ///     the value is null or does not conform to a comma-seperated string containing two decimal points. This parameter is
        ///     passed uninitialized.
        /// </param>
        /// <returns>true if the convertion succeeded; otherwise false.</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="FormatException" />
        public static bool TryParse(string value, out LatLng result)
        {
            result = null;
            if (value == null) return false;

            try
            {
                result = Parse(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LatLng);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash += hash * 7 + Latitude.GetHashCode();
            hash += hash * 7 + Longitude.GetHashCode();
            return hash;
        }
    }
}