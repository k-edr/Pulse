namespace IngameScript.Pulse.CommandInteface
{
    public struct ArgumentProvider : IDataProvider
    {
        public string _data;

        private bool _isEmpty;

        public ArgumentProvider(string data)
        {
            _data = data;

            if(_data == string.Empty)
            {
                _isEmpty = true;
            }

            _isEmpty = false;
        }

        public bool IsEmpty => _isEmpty;
        
        public string Get => _data;
    }
}
