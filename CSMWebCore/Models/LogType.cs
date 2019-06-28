using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public enum LogType
    {
        InitialInspection,
        ContactCustomer,
        GeneralTroubleshooting,
        HardwareTroubleshooting,
        HardwareReplacement,
        DataBackup,
        DataRestore,
        SoftwareInstallation,
        SoftwareUpdates,
        OSInstallation,
        OSUpdate,
        OfficeInstallation,
        MalwareScan,
        MalwareRemoval
    }
}
