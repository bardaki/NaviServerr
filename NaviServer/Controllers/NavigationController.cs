using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class NavigationController : ApiController
    {
        private readonly String GOOGLE_NAVIGATION = "https://maps.googleapis.com/maps/api/directions/json?origin=";
        private List<List<Route>> navigationOptions = new List<List<Route>>();
        private List<Route> routes = new List<Route>();
        private Navigation nav = new Navigation();
        private int minTimeIndex = 0;

        [AcceptVerbs("GET", "POST")]
        public List<Route> Get(Navigation navigation)//Navigation navigation
        {
            nav = navigation;
            //getJSONFromUrl("http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0=32.069722,34.766597&wp.1=32.1929421,34.8851231&avoid=minimizeTolls&key=AtQ_t8sdaxDTeF7X8TZxTV8JShIlPaNgqasTjzXW1xSQUkwSHHljWov-9ikBMyHG");
            //main algorithm to find the best route
            if (nav.getAddresses().Count() != 0)
            {
                //Get all pairs options
                getAllPairs();
                //Get start routes
                getStartRoutes();
                //Get routes
                while (navigationOptions.ElementAt(0).Count() < nav.getAddresses().Count())
                {
                    navigationOptions = getRoutes();
                }
                //Get end routes
                getEndRoutes();
                minTimeIndex = getShortestRoute();
            }
            else
            {
                String urlString = GOOGLE_NAVIGATION + nav.getStartAdd() + "&destination=" + nav.getEndAdd() + "&region=il";
                List<Route> onlyRoute = new List<Route>();
                Route r = new Route(nav.getStartAdd(), nav.getEndAdd(), getJSONFromUrl(urlString).ElementAt(0), getJSONFromUrl(urlString).ElementAt(1));
                onlyRoute.Add(r);
                navigationOptions.Add(onlyRoute);
            }
            return null;
        }

        //Get all pairs options
        public void getAllPairs()
        {
            for (int i = 0; i < nav.getAddresses().Count(); i++)
            {
                //Add routes from start address
                String urlString = GOOGLE_NAVIGATION + nav.getStartAdd() + "&destination=" + nav.getAddresses().ElementAt(i) + "&region=il";
                Route rt = new Route(nav.getStartAdd(), nav.getAddresses().ElementAt(i), getJSONFromUrl(urlString).ElementAt(0), getJSONFromUrl(urlString).ElementAt(1));
                routes.Add(rt);
                //Add all routes without start & end addresses
                if (i != nav.getAddresses().Count() - 1)
                {
                    for (int j = i + 1; j < nav.getAddresses().Count(); j++)
                    {
                        urlString = GOOGLE_NAVIGATION + nav.getAddresses().ElementAt(i) + "&destination=" + nav.getAddresses().ElementAt(j) + "&region=il";
                        rt = new Route(nav.getAddresses().ElementAt(i), nav.getAddresses().ElementAt(j), getJSONFromUrl(urlString).ElementAt(0), getJSONFromUrl(urlString).ElementAt(1));
                        urlString = GOOGLE_NAVIGATION + nav.getAddresses().ElementAt(i) + "&destination=" + nav.getAddresses().ElementAt(j) + "&region=il";
                        Route rt2 = new Route(nav.getAddresses().ElementAt(j), nav.getAddresses().ElementAt(i), getJSONFromUrl(urlString).ElementAt(0), getJSONFromUrl(urlString).ElementAt(1));
                        routes.Add(rt);
                        routes.Add(rt2);
                    }
                }
                //Add routes to end address
                urlString = GOOGLE_NAVIGATION + nav.getAddresses().ElementAt(i) + "&destination=" + nav.getEndAdd() + "&region=il";
                rt = new Route(nav.getAddresses().ElementAt(i), nav.getEndAdd(), getJSONFromUrl(urlString).ElementAt(0), getJSONFromUrl(urlString).ElementAt(1));
                routes.Add(rt);
            }
        }

        //Add start address routes
        public void getStartRoutes()
        {
            for (int i = 0; i < routes.Count(); i++)
            {
                List<Route> list = new List<Route>();
                if (routes.ElementAt(i).getSource() == nav.getStartAdd())
                {
                    list.Add(routes.ElementAt(i));
                    navigationOptions.Add(list);
                }
            }
        }

        //Add end address routes
        public void getEndRoutes()
        {
            for (int i = 0; i < navigationOptions.Count(); i++)
            {
                for (int j = 0; j < routes.Count(); j++)
                {
                    if (navigationOptions.ElementAt(i).ElementAt(navigationOptions.ElementAt(i).Count() - 1).getDestination() == routes.ElementAt(j).getSource() &&
                            routes.ElementAt(j).getDestination() == nav.getEndAdd())
                    {
                        navigationOptions.ElementAt(i).Add(routes.ElementAt(j));
                    }
                }
            }
        }

        public List<String> getJSONFromUrl(String urll)
        {
            //var json = new WebClient().DownloadString(urll);
            List<String> result = new List<String>();

            return null;
        }

        public List<List<Route>> getRoutes()
        {
            int sum = 0;
            int minTime = Int32.MaxValue;
            //Add routes
            List<List<Route>> navigationOptions2 = new List<List<Route>>();
            for (int i = 0; i < navigationOptions.Count(); i++)
            {
                for (int j = 0; j < routes.Count(); j++)
                {
                    if (navigationOptions.ElementAt(i).ElementAt(navigationOptions.ElementAt(i).Count() - 1).getDestination() == routes.ElementAt(j).getSource() &&
                            navigationOptions.ElementAt(i).ElementAt(navigationOptions.ElementAt(i).Count() - 1).getSource() != routes.ElementAt(j).getDestination() && routes.ElementAt(j).getDestination() != nav.getEndAdd())
                    {
                        if (!contains(navigationOptions.ElementAt(i), routes.ElementAt(j).getDestination()))
                        {
                            List<Route> list = new List<Route>();
                            for (int k = 0; k < navigationOptions.ElementAt(i).Count(); k++)
                            {
                                list.Add(navigationOptions.ElementAt(i).ElementAt(k));
                                sum += navigationOptions.ElementAt(i).ElementAt(k).getDuration();
                                if (sum >= minTime)
                                    break;
                            }
                            if (sum < minTime)
                            {
                                minTime = sum;
                                //indexMin = i;
                                list.Add(routes.ElementAt(j));
                                navigationOptions2.Add(list);
                            }
                            sum = 0;
                            //						list.add(routes.get(j));
                            //						navigationOptions2.add(list);
                        }
                    }
                }
            }
            return navigationOptions2;
        }

        public bool contains(List<Route> list, String address)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                if (list.ElementAt(i).getSource() == address || list.ElementAt(i).getDestination() == address)
                    return true;
            }
            return false;
        }

        public int getShortestRoute()
        {
            int i = 0;
            int sum = 0;
            int minTime = Int32.MaxValue;
            int indexMin = 0;
            for (i = 0; i < navigationOptions.Count(); i++)
            {
                for (int j = 0; j < navigationOptions.ElementAt(i).Count(); j++)
                {
                    sum += navigationOptions.ElementAt(i).ElementAt(j).getDuration();
                    if (sum >= minTime)
                        break;
                }
                if (sum < minTime)
                {
                    minTime = sum;
                    indexMin = i;
                }
                sum = 0;
            }
            return indexMin;
        }
    }
}
