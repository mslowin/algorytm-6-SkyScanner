using System.Text.RegularExpressions;

namespace _algorytm_6__SkyScanner
{
    internal static class Program
    {
        /// <summary>
        /// List of all flights that haven't been processed yet.
        /// </summary>
        public static List<Flight> _flights = new();

        /// <summary>
        /// List of locations, history of visited places
        /// </summary>
        public static List<string> _fromTmp = new();

        /// <summary>
        /// List of total prices from each route to destination
        /// </summary>
        public static List<int> _costsOfAllFlightsToFinalDestination = new();

        /// <summary>
        /// List of total numbers of stopovers for each route. MAches total prices from _costsOfAllFlightsToFinalDestination
        /// </summary>
        public static List<int> _allStopOvers = new();

        /// <summary>
        /// List of strings showing how the routes go.
        /// </summary>
        public static List<string> _routes = new();

        /// <summary>
        /// Helps keep track of how many times finalDestination was reached in algorithm
        /// </summary>
        public static int _destinationReachedCounter = 0;

        /// <summary>
        /// Temporary variable storing total price of one route. It is added to the list _costsOfAllFlightsToFinalDestination
        /// </summary>
        public static int _travelCost = 0;

        /// <summary>
        /// Temporary variable storing total number of stopovers. It is added to the list _allStopOvers
        /// </summary>
        public static int _stopOvers = 0;



        private static void Main()
        {
            Flight[] input = { new Flight("JFK", "ATL", 150), 
                               new Flight("ATL", "SFO", 400), 
                               new Flight("ORD", "LAX", 200),
                               new Flight("LAX", "DFW", 80),  
                               new Flight("JFK", "HKG", 800), 
                               new Flight("ATL", "ORD", 90),
                               new Flight("JFK", "LAX", 500) };

            List<Flight> flights = new();
            flights.AddRange(input);

            string from = "JFK";
            string finalDestination = "LAX";

            _flights = flights.GetRange(0, flights.Count);

            int numOfFlightsToDestination = FindAllFlightsToFinalDestination(flights, finalDestination); // getting total number of flights directly to the final destination

            _costsOfAllFlightsToFinalDestination = new List<int>(numOfFlightsToDestination);
            _allStopOvers = new List<int>(numOfFlightsToDestination);

            SearchBranch(flights.Count, from, flights, finalDestination, numOfFlightsToDestination); // searches for connections to the final destionation

            var maxStopOvers = 3;

            DisplayTableOfFlights(); // displaying table of all posible routes
            CalculateAndDisplayBestRoutes(maxStopOvers); // calculating which route meets the requirements
        }

        /// <summary>
        /// Searches through each possible route in a tree and finds the ones that start and end on specified locations. Calculates prices and stopovers.
        /// </summary>
        /// <param name="flightsCount">Total number of flights that haven't been tracked yet.</param>
        /// <param name="from1">Starting point of a flight.</param>
        /// <param name="flightsTmp">List of flights that haven't been tracked yet.</param>
        /// <param name="finalDestination">Desired final destination of a whole journey</param>
        /// <param name="numOfFlightsToDestination">Number of flights that end in the final destination.</param>
        public static void SearchBranch(int flightsCount, string from1, List<Flight> flightsTmp, string finalDestination, int numOfFlightsToDestination)
        {
            var counter = 0;
            for (int i = 0; i < flightsCount; i++)
            {
                if (flightsTmp[i]._from != from1) // jeśli aktualnie sprawdzane połączenie nie zaczyna się tam gdzie kończyło się we wcześniejszym wywołaniu funkcji
                {
                    counter++;
                    if (counter == flightsCount) // jeśli po przeszukaniu wszystkich lotów dalej nie znajdzie dopasowania, to znaczy, że to koniec gałęzi
                    {
                        //Console.WriteLine(from1); // aktualnie gałąź kończy się tutaj
                        for (int j = 0; j < _flights.Count; j++)
                        {
                            if (_flights[j]._from == _fromTmp[_fromTmp.Count - 1] && _flights[j]._destination == from1) // usuwamy aktualne połączenie z listy globalnej, bo jest końcem gałęzi
                            {
                                _flights.Remove(_flights[j]);
                                break;
                            }
                        }
                    }
                }
                else // jeśli znajdzie połączenie, które zaczyna się tam, gdzie kończyło się w poprzednim wywołaniu funkcji
                {

                    _fromTmp.Add(from1);
                    from1 = flightsTmp[i]._destination; // nowy początek, to koniec poprzedniego połączenia
                    _travelCost += flightsTmp[i]._cost; // dodajemy koszt kolejnego połączenia
                    _stopOvers++;


                    if (flightsTmp[i]._destination == finalDestination) // jeśli trafimy do naszej destynacji
                    {
                        //Console.WriteLine("Jestes na miejscu");
                        _costsOfAllFlightsToFinalDestination.Add(_travelCost); // odkładamy kwotę do listy
                        _allStopOvers.Add(_stopOvers - 1); // odkładamy do listy ilość przesiadek zmniejszoną o 1, bo liczba przesiadek jest zawsze o jeden mniejsza od ilosci połączeń
                        _flights.Remove(_flights.Where(x => x._from == flightsTmp[i]._from && x._destination == flightsTmp[i]._destination).ToList()[0]); // usuwamy połączenie z listy globalnej, żeby już tam nie trafić
                        _destinationReachedCounter++;
                        string route = "";


                        route += finalDestination;
                        for (int x = _fromTmp.Count - 1; x >= 0; x--)
                        {
                            route += " <- " + _fromTmp[x]; // string builder!
                            if (_fromTmp[x] == _fromTmp[0])
                            {
                                //RevereseString(route);
                                _routes.Add(route);
                                break;
                            }
                        }
                    }
                    if (_destinationReachedCounter == numOfFlightsToDestination) // jeśli dojdziemy do destynacji z każdej możliwej strony, to przerywamy
                    {
                        return;
                    }

                    flightsTmp.Remove(flightsTmp[i]);

                    SearchBranch(flightsTmp.Count, from1, flightsTmp, finalDestination, numOfFlightsToDestination);


                    if (_destinationReachedCounter == numOfFlightsToDestination) // jeśli przerwaliśmy funkcję znajdując wszystkie ścieżki do destynacji, to przerywamy również wszystkie funkcje nadrzędne
                    {
                        return; // tu finalnie kończy się działanie całej funkcji
                    }

                    from1 = _fromTmp[0]; // ponownie zaczynamy od lokacji początkowej
                    _travelCost = 0; // koszt podróży liczymy od początku
                    _stopOvers = 0;
                    flightsTmp = _flights.GetRange(0, _flights.Count); // pobieramy nową listę lotów, w której nie ma końców gałęzi i lotów kończących się w destynacji
                    i = -1; // żeby pętla for zaczęła się od zera
                }
                
            }
        }

        /// <summary>
        /// Displays table of all avaliable routes to the final destination
        /// </summary>
        public static void DisplayTableOfFlights()
        {
            var tmp = _routes.GetRange(0, _routes.Count);
            tmp.Sort();
            var numOfLocationsInTheLongestRoute = tmp[tmp.Count - 1].Split(" <- ").Count();
            var spacing = 4 * (numOfLocationsInTheLongestRoute - 1) + 3 * numOfLocationsInTheLongestRoute; // number of characters in the longest route

            Console.WriteLine("All posible routs:");
            Console.WriteLine(String.Format("{0, " + spacing + "}", "ROUTE:") + "\tSTOPOVERS:\tPRICE:");

            for (int i = 0; i < _routes.Count; i++)
            {
                Console.WriteLine(String.Format("{0, " + spacing + "}", _routes[i].ToString()) + "\t" + _allStopOvers[i].ToString() + "\t\t" + _costsOfAllFlightsToFinalDestination[i].ToString()); ;
            }


        }

        /// <summary>
        /// Calculates which route has the lowest price and meets stopover requirement
        /// </summary>
        /// <param name="maxStopOvers">Maximum allowed stopovers</param>
        public static void CalculateAndDisplayBestRoutes(int maxStopOvers)
        {
            for (int i = 0; i < _allStopOvers.Count; i++)
            {
                if (_allStopOvers[i] > maxStopOvers) // removing all routes that don't meet stopover requirement
                {
                    _allStopOvers.RemoveAt(i);
                    _costsOfAllFlightsToFinalDestination.RemoveAt(i);
                    _routes.RemoveAt(i);
                }
            }

            List<int> tmp = _costsOfAllFlightsToFinalDestination.GetRange(0, _costsOfAllFlightsToFinalDestination.Count); // temporary list to be sorted
            tmp.Sort();
            var minPrice = tmp[0]; // minimal price is the one with 0 index in sorted list
            List<int> MinPriceIndexes = new(); // there can be more than one routes with same total price so it is a List

            for (int i = 0; i < _costsOfAllFlightsToFinalDestination.Count; i++)
            {
                if (_costsOfAllFlightsToFinalDestination[i] == minPrice) // adding to a list indexes of all routes which prices are equal to the lowest price possible
                {
                    MinPriceIndexes.Add(i);
                }
            }

            foreach (var item in MinPriceIndexes) // displaying all routes which meet the requirements and have the same lowest possible price
            {
                Console.WriteLine("\n" + _routes[item]);
                Console.WriteLine("Cena: " + _costsOfAllFlightsToFinalDestination[item] + "\n");
            }

        }

        /// <summary>
        /// Finds how many flightsTmp end in a specified location.
        /// </summary>
        /// <param name="flights">list of all flightsTmp.</param>
        /// <param name="finalDestination">location to  search for.</param>
        /// <returns>Number of flightsTmp that end in the finalDestination</returns>
        public static int FindAllFlightsToFinalDestination(List<Flight> flights, string finalDestination)
        {
            var counter = 0;
            foreach (var item in flights)
            {
                if (item._destination == finalDestination)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    /// <summary>
    /// Class stores information about a single flight.
    /// </summary>
    internal class Flight
    {
        /// <summary>
        /// Starting location of a flight.
        /// </summary>
        public readonly string _from;

        /// <summary>
        /// Flight destination.
        /// </summary>
        public readonly string _destination;

        /// <summary>
        /// Price of the flight.
        /// </summary>
        public readonly int _cost;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="from">Starting point of a flight.</param>
        /// <param name="destination">Flight destination.</param>
        /// <param name="cost">Price of a flight.</param>
        public Flight(string from, string destination, int cost)
        {
            _from = from;
            _destination = destination;
            _cost = cost;
        }
    }
}