using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WebApi.Models
{
    [DataContract]
    public class Route
    {
        [DataMember]
        public String source;
        [DataMember]
        public String destination;
        [DataMember]
        public int endLatitude;
        [DataMember]
        public int endLongitude;
        [DataMember]
        public int duration;
        [DataMember]
        public String durationText;

        public Route(String source, String destination, String duration, String durationText) //, int endLatitude, int endLongitude
        {
            this.source = source;
            this.destination = destination;
            this.duration = Convert.ToInt32(duration);
            this.durationText = durationText;
            //this.endLatitude = endLatitude;
            //this.endLongitude = endLongitude;
        }

        public String getSource()
        {
            return source;
        }

        public String getDestination()
        {
            return destination;
        }

        public int getDuration()
        {
            return duration;
        }

        public int getLatitude()
        {
            return endLatitude;
        }

        public int getLongitude()
        {
            return endLongitude;
        }

        public String toString()
        {
            char[] delimiterChars = { ',' };
            String[] src = source.Split(delimiterChars);
            String[] dst = destination.Split(delimiterChars);

            return src[0] + "- " + dst[0] + "\n          " + durationText;
        }
    }
}