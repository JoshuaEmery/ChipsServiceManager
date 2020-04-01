using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public enum LogType
    {
        CheckIn,
        Contact,
        Checkout,
        Diagnostic,
        OsInstallation,
        OsUpdate,
        DataBackupRestore,      
        SoftwareInstallation,
        DriveInstallation,
        RamInstallation,
        ScreenReplacement,
        KeyboardReplacement,
        TouchpadReplacement,
        HingeRepair,
        VirusRemoval,
        BatteryReplacment,
        PowerJackReplacement,               
        MiscRepair        
    }
    /*
     * CheckIn - Not available through views, automatic when creating a new ticket
     * Diagnostic - $50 // This is only charged if nothing else is charged
     * Contact - Not available through views, automatic when logging contact
     * OsInstallation - $150
     * OsUpdate - $50
     * SoftwareInstallation - $50
     * ScreenReplacement - $150
     * KeyboardReplacement - $125
     * TouchPadReplacement - $150
     * HingeRepair - $150
     * VirusRemoval - $150
     * DataBackup/Restore - $150
     * BatteryReplacment - $50
     * PowerJackReplacement - $150
     * RamUpgrade - $100
     * DriveInstallation - $100  
     * MiscRepair - $100 
     * Checkout // Not available through views, automic on pickup log 
     * 
     */

}
