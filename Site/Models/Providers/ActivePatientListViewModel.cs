using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllAcu.Models.Patients
{
    public class ActivePatientListViewModel
    {
        public IList<PatientPersonalInformation> Patients { get; set; }
    }
}
