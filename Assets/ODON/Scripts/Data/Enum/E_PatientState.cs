namespace ODON.Data
{
    /// <summary>
    /// Enumeration representing the possible states of a patient in the ODON system.
    /// </summary>
    public enum PatientState
    {
        /// <summary>
        /// Patient is in the waiting room.
        /// </summary>
        InWaitingRoom,
        /// <summary>
        /// Patient is in the cabinet.
        /// </summary>
        InCabinet,
        /// <summary>
        /// Patient is in bed.
        /// </summary>
        InBed,
        /// <summary>
        /// Patient is leaving.
        /// </summary>
        Leaving,
    }
}