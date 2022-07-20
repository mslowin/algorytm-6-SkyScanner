namespace _algorytm_6__SkyScanner
{
    internal static class Program
    {
        public static List<Flight>? _flightsTmp;
        public static List<string>? _fromTmp = new();
        public static int _index = 0;

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
            string fromTemp;
            string destination = "LAX";

            _flightsTmp = flights.GetRange(0, flights.Count);
            SearchBranch(flights.Count, from, flights, 99);

        }

        public static void SearchBranch(int count, string from1, List<Flight> flights, int remove)
        {
            var counter = 0;
            for (int i = 0; i < count; i++)
            {
                else if (flights[i]._from != from1) // tutaj form1 powinno znowu być ALT !!!
                {
                    counter++;
                    // koniec galezi
                    if (counter == count)
                    {
                        Console.WriteLine(from1);
                        for (int j = 0; j < _flightsTmp.Count; j++)
                        {
                            if (_flightsTmp[j]._from == _fromTmp[_fromTmp.Count - 1] && _flightsTmp[j]._destination == from1) // !!!!!!!!!!!!!!!
                            {

                                _flightsTmp.Remove(_flightsTmp[j]);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    _fromTmp.Add(from1);
                    from1 = flights[i]._destination; // tutaj jeśli destination == "LAX" to znaczy, że jesteśmy na miejscu
                    flights.Remove(flights[i]);

                    _index++;

                    SearchBranch(flights.Count, from1, flights, i);

                    from1 = _fromTmp[0]; // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! moze, zeby skakalo tylko jeden do gory?
                    flights = _flightsTmp.GetRange(0, _flightsTmp.Count);
                    i = -1;
                }
                
            }
        }
    }

    internal class Flight
    {
        public readonly string _from;
        public readonly string _destination;
        public readonly int _cost;

        public Flight(string from, string destination, int cost)
        {
            _from = from;
            _destination = destination;
            _cost = cost;
        }
    }
}