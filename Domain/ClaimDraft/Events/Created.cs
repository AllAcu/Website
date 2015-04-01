using Microsoft.Its.Domain;

namespace Domain
{
    public class Created : Event<ClaimDraft>
    {
        public Created(string diagnosis)
        {
            Diagnosis = diagnosis;
        }

        public string Diagnosis { get; }

        public override void Update(ClaimDraft draft)
        {
            draft.Diagnosis = Diagnosis;
        }
    }
}
