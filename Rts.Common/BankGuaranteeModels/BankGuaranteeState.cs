namespace Rts.Common.BankGuaranteeModels
{
    public enum BankGuaranteeState
    {
        None = 1,

        CreatedAsDraft = 5,

        ApprovedByExpert = 10,
        RejectedByExpert = 12,

        ApprovedByPM = 15,
        RejectedByPm = 17
    }

}
