using HarmonyLib;
using static TownOfHost.Translator;
using UnityEngine;

namespace TownOfHost
{
    public class AdminPatch
    {
        //参考元 : https://github.com/yukinogatari/TheOtherRoles-GM/blob/gm-main/TheOtherRoles/Patches/AdminPatch.cs
        public static bool DisableAdmin()
        {
            var ArchiveAdminDistance = Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), new Vector2(20.0f, 12.3f));

            var DisableAllAdmins = Options.WhichDisableAdmin.GetString() == GetString(Options.whichDisableAdmin[0]); //すべてのアドミン無効化
            var DisableArchiveAdmin = PlayerControl.GameOptions.MapId != 4 && Options.WhichDisableAdmin.GetString() == GetString(Options.whichDisableAdmin[1]); //エアシップ時、アーカイブアドミンのみ無効化
            return Options.DisableAdmin.GetBool() && (DisableAllAdmins || (DisableArchiveAdmin && ArchiveAdminDistance <= DisableDevice.UsableDistance(PlayerControl.GameOptions.MapId))); //DisableAdminが有効で、DisableAllAdminsかDisableArchiveAdminが有効の時のみアドミン無効化
        }
        [HarmonyPatch(typeof(MapConsole), nameof(MapConsole.Use))]
        public static class MapConsoleUsePatch
        {
            public static bool Prefix()
            {
                var DisableAllAdmins = Options.WhichDisableAdmin.GetString() == GetString(Options.whichDisableAdmin[0]) ||
                    !(PlayerControl.GameOptions.MapId != 4 && Options.WhichDisableAdmin.GetString() == GetString(Options.whichDisableAdmin[1])); //エアシップ以外でアーカイブのみ制限になっていたら制限しない
                var DisableArchiveAdmin = Options.WhichDisableAdmin.GetString() == GetString(Options.whichDisableAdmin[1]);
                var ArchiveAdminDistance = Vector2.Distance(PlayerControl.LocalPlayer.transform.position, new Vector2(20.0f, 12.3f));
                var DisableAdmin =
                Options.DisableAdmin.GetBool() && (DisableAllAdmins || (DisableArchiveAdmin && ArchiveAdminDistance <= 1.5));
                return !DisableAdmin;
            }
        }
    }
}