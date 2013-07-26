using System;

namespace PuttyRun {
    internal enum PuttyConnectionType {
        Unknown = 0,
        Raw = 1,
        Telnet = 2,
        RLogin = 3,
        Ssh = 4,
        Serial = 5,
    }
}
