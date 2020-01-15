namespace TestManagementServices.Model
{
    public class RankApplicantStatistic
    {
        public int value { get; set; }
        private string _name;

        public string name
        {
            get { return _name; }
            set { _name = value ?? "Not ranked"; }
        }
    }
}
