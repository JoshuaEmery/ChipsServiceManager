using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public enum EventEnum
    {
        CheckIn = 1,
        CheckOut = 2,
        LostandFound = 3,
        Diagnostic = 4,
        DataBackup = 5,
        DataRestore = 6,
        OSInstallation = 7,
        OSUpdates = 8,
        ProgramDriverInstallation = 9,
        VirusMalwareRemoval = 10,
        MiscSoftware = 11,
        BatteryReplacement = 12,
        DisplayReplacement = 13,
        HingeReplacement = 14,
        KeyboardReplacement = 15,
        PowerJackReplacement = 16,
        RAMReplacement = 17,
        StorageDriveReplacement = 18,
        TrackpadReplacement = 19,
        MiscHardware = 20,
        InPerson = 21,
        Email = 22,
        PhoneCall = 23,
        Voicemail = 24
    }
}
