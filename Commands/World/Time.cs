﻿using System.Text.RegularExpressions;
using Terraria;
using Terraria.ModLoader;

namespace CheatCommands.Commands.World {
    class Time : CheatCommand {
        public override string CommandName => "Time";
        public override string Command => "time";
        public override string Usage => base.Usage + " <dawn/dusk/noon/midnight/time>";
        public override string Description => "Change the time of day.";
        public override int MinimumArguments => 1;

        public override void Action(CommandCaller caller, string[] args) {
            int hours = 0;
            int minutes = 0;
            bool succeeded = true;

            switch(args[0]) {
                case "dawn":
                    ToTwentyFourHourTime("4:30 AM", out hours, out minutes);
                    break;
                case "noon":
                    ToTwentyFourHourTime("12:00 PM", out hours, out minutes);
                    break;
                case "dusk":
                    ToTwentyFourHourTime("7:30 PM", out hours, out minutes);
                    break;
                case "midnight":
                    ToTwentyFourHourTime("12:00 AM", out hours, out minutes);
                    break;
                default:
                    succeeded = ToTwentyFourHourTime(args[0], out hours, out minutes);
                    break;
            }
            
            if(succeeded) {
                bool freezeTime = CheatCommands.TimeFrozen;
                CheatCommands.TimeFrozen = false;
                
                ChangeTime(hours, minutes);

                if(freezeTime) {
                    CheatCommands.TimeFrozen = true;
                }
            }
            else {
                throw new UsageException("Invalid time format: " + args[0]);
            }
        }

        private static bool ToTwentyFourHourTime(string timeString, out int hours, out int minutes) {
            Regex twentyFourHour = new Regex(@"^([01]?\d|2[0-3]):([0-5]\d)");
            Regex twelveHour = new Regex(@"^(1[0-2]|0?\d):([0-5]\d) ([AaPp][Mm])");

            Match twentyFourHourMatch = twentyFourHour.Match(timeString);
            Match twelveHourMatch = twelveHour.Match(timeString);

            int finalHours = 0;

            if(!twentyFourHourMatch.Success && !twelveHourMatch.Success) {
                hours = 0;
                minutes = 0;
                return false;
            }

            if(twelveHourMatch.Success) {
                finalHours = int.Parse(twelveHourMatch.Groups[1].Value);
                bool am = twelveHourMatch.Groups[3].Value.ToLower().Equals("am");

                if(am && finalHours == 12) {
                    finalHours -= 12;
                }
                else if(!am && finalHours < 12) {
                    finalHours += 12;
                }
            }
            else if(twentyFourHourMatch.Success) {
                finalHours = int.Parse(twentyFourHourMatch.Groups[1].Value);
            }

            hours = finalHours;
            minutes = int.Parse(twentyFourHourMatch.Groups[2].Value);
            return true;
        }

        private static void ChangeTime(double hours, double minutes) {
            bool dayTime = false;
            double time = 0.0;

            minutes = minutes / 60.0;
            time = hours + minutes;

            if(time >= 4.5 && time < 19.5) {
                dayTime = true;
            }

            time = time + 7.5 + 12.0;

            if(time >= 24.0) {
                time -= 24.0;
            }

            time = time * 86400.0 / 24.0;

            if(!dayTime && time >= 54000.0) {
                time -= 54000.0;
            }

            Main.dayTime = dayTime;
            Main.time = time;
        }
    }
}
