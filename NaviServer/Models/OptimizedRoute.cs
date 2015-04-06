using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace NaviServer.Models
{
    public class OptimizedRoute
    {
        public List<Route> optimizedRoute { get; set; }
        public int totalTime { get; set; }

        public OptimizedRoute()
        {
            this.optimizedRoute = new List<Route>();
        }

        public OptimizedRoute(List<Route> route, int totalTime) {
            this.optimizedRoute = route;
            this.totalTime = totalTime;
        }
    }
}