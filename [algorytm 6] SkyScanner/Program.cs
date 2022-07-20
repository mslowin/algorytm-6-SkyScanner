namespace _algorytm_6__SkyScanner
{
    internal static class Program
    {
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