﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ADcontrol
{
    class TerminalServicesInfo
    {
        #region Constants
        public const int WTS_CURRENT_SESSION = -1;
        #endregion

        #region Dll Imports
        [DllImport("wtsapi32.dll")]
        static extern int WTSEnumerateSessions(
            IntPtr pServer,
            [MarshalAs(UnmanagedType.U4)] int iReserved,
            [MarshalAs(UnmanagedType.U4)] int iVersion,
            ref IntPtr pSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref int iCount);

        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSQuerySessionInformation(
            System.IntPtr pServer,
            int iSessionID,
            WTS_INFO_CLASS oInfoClass,
            out System.IntPtr pBuffer,
            out uint iBytesReturned);

        [DllImport("wtsapi32.dll")]
        static extern void WTSFreeMemory(
            IntPtr pMemory);
        #endregion

        #region Structures
        //Structure for Terminal Service Client IP Address
        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_CLIENT_ADDRESS
        {
            public int iAddressFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] bAddress;
        }

        //Structure for Terminal Service Session Info
        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_SESSION_INFO
        {
            public int iSessionID;
            [MarshalAs(UnmanagedType.LPStr)]
            public string sWinsWorkstationName;
            public WTS_CONNECTSTATE_CLASS oState;
        }

        //Structure for Terminal Service Session Client Display
        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_CLIENT_DISPLAY
        {
            public int iHorizontalResolution;
            public int iVerticalResolution;
            //1 = The display uses 4 bits per pixel for a maximum of 16 colors.
            //2 = The display uses 8 bits per pixel for a maximum of 256 colors.
            //4 = The display uses 16 bits per pixel for a maximum of 2^16 colors.
            //8 = The display uses 3-byte RGB values for a maximum of 2^24 colors.
            //16 = The display uses 15 bits per pixel for a maximum of 2^15 colors.
            public int iColorDepth;
        }
        #endregion

        #region Enumurations
        public enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        public enum WTS_INFO_CLASS
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType,
            WTSIdleTime,
            WTSLogonTime,
            WTSIncomingBytes,
            WTSOutgoingBytes,
            WTSIncomingFrames,
            WTSOutgoingFrames,
            WTSClientInfo,
            WTSSessionInfo,
            WTSConfigInfo,
            WTSValidationInfo,
            WTSSessionAddressV4,
            WTSIsRemoteSession
        }
        #endregion

        static void GetTerminalServicesInfo(string[] args)
        {
            IntPtr pServer = IntPtr.Zero;
            string sUserName = string.Empty;
            string sDomain = string.Empty;
            string sClientApplicationDirectory = string.Empty;
            string sIPAddress = string.Empty;

            WTS_CLIENT_ADDRESS oClientAddres = new WTS_CLIENT_ADDRESS();
            WTS_CLIENT_DISPLAY oClientDisplay = new WTS_CLIENT_DISPLAY();

            IntPtr pSessionInfo = IntPtr.Zero;

            int iCount = 0;
            int iReturnValue = WTSEnumerateSessions
                (pServer, 0, 1, ref pSessionInfo, ref iCount);
            int iDataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));

            int iCurrent = (int)pSessionInfo;

            if (iReturnValue != 0)
            {
                //Go to all sessions
                for (int i = 0; i < iCount; i++)
                {
                    WTS_SESSION_INFO oSessionInfo =
            (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)iCurrent,
                typeof(WTS_SESSION_INFO));
                    iCurrent += iDataSize;

                    uint iReturned = 0;

                    //Get the IP address of the Terminal Services User
                    IntPtr pAddress = IntPtr.Zero;
                    if (WTSQuerySessionInformation(pServer,
            oSessionInfo.iSessionID, WTS_INFO_CLASS.WTSClientAddress,
            out pAddress, out iReturned) == true)
                    {
                        oClientAddres = (WTS_CLIENT_ADDRESS)Marshal.PtrToStructure
                    (pAddress, oClientAddres.GetType());
                        sIPAddress = oClientAddres.bAddress[2] + "." +
                oClientAddres.bAddress[3] + "." + oClientAddres.bAddress[4]
                + "." + oClientAddres.bAddress[5];
                    }
                    //Get the User Name of the Terminal Services User
                    if (WTSQuerySessionInformation(pServer,
            oSessionInfo.iSessionID, WTS_INFO_CLASS.WTSUserName,
                out pAddress, out iReturned) == true)
                    {
                        sUserName = Marshal.PtrToStringAnsi(pAddress);
                    }
                    //Get the Domain Name of the Terminal Services User
                    if (WTSQuerySessionInformation(pServer,
            oSessionInfo.iSessionID, WTS_INFO_CLASS.WTSDomainName,
                out pAddress, out iReturned) == true)
                    {
                        sDomain = Marshal.PtrToStringAnsi(pAddress);
                    }
                    //Get the Display Information  of the Terminal Services User
                    if (WTSQuerySessionInformation(pServer,
                oSessionInfo.iSessionID, WTS_INFO_CLASS.WTSClientDisplay,
                out pAddress, out iReturned) == true)
                    {
                        oClientDisplay = (WTS_CLIENT_DISPLAY)Marshal.PtrToStructure
                (pAddress, oClientDisplay.GetType());
                    }
                    //Get the Application Directory of the Terminal Services User
                    if (WTSQuerySessionInformation(pServer, oSessionInfo.iSessionID,
            WTS_INFO_CLASS.WTSClientDirectory, out pAddress, out iReturned) == true)
                    {
                        sClientApplicationDirectory = Marshal.PtrToStringAnsi(pAddress);
                    }

                    Console.WriteLine("Session ID : " + oSessionInfo.iSessionID);
                    Console.WriteLine("Session State : " + oSessionInfo.oState);
                    Console.WriteLine("Workstation Name : " +
                oSessionInfo.sWinsWorkstationName);
                    Console.WriteLine("IP Address : " + sIPAddress);
                    Console.WriteLine("User Name : " + sDomain + @"\" + sUserName);
                    Console.WriteLine("Client Display Resolution: " +
            oClientDisplay.iHorizontalResolution + " x " +
                oClientDisplay.iVerticalResolution);
                    Console.WriteLine("Client Display Colour Depth: " +
                oClientDisplay.iColorDepth);
                    Console.WriteLine("Client Application Directory: " +
                sClientApplicationDirectory);

                    Console.WriteLine("-----------------------");
                }

                WTSFreeMemory(pSessionInfo);
            }
        }
    }
}