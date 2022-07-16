using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using ConsoleTableExt;

namespace BPR_Mod_Manager
{
    class VehicleMod 
    {
        // Vehicle mod class
        private string modName, replacedCarID, replacedCarName;
        private bool isAT, isCD, isGR, isActive;

        public VehicleMod(string fileName, string directory, bool active)
        {
            string database = File.ReadAllText(Path.Combine(Convert.ToString(Directory.GetCurrentDirectory()), "DATA\\VehicleDatabase.txt"));

            modName = fileName;
            string tempPath = Directory.GetFiles(directory + "\\" + fileName, "*.BIN")[0];
            replacedCarID = tempPath.Substring(tempPath.LastIndexOf('\\') + 1).Split('_')[1];

            replacedCarName = "ERROR";

            isActive = active;

            if (File.Exists(directory + "/" + fileName + "/VEH_" + replacedCarID + "_AT.BIN")) isAT = true;
            else isAT = false;
            if (File.Exists(directory + "/" + fileName + "/VEH_" + replacedCarID + "_CD.BIN")) isCD = true;
            else isCD = false;
            if (File.Exists(directory + "/" + fileName + "/VEH_" + replacedCarID + "_GR.BIN")) isGR = true;
            else isGR = false;

            foreach (string car in database.Split('\n'))
            {
                // Iterates through the vehicle database to find the associated vehicle name
                if (car.Split(',')[1] == replacedCarID)
                {
                    replacedCarName = car.Split(',')[0];
                }
            }
        }

        public string GetName()
        {
            return modName;
        }

        public string GetReplacedCar()
        {
            return replacedCarName;
        }

        public bool GetIsAT()
        {
            return isAT;
            // Attributes
        }
        public bool GetIsCD()
        {
            return isCD;
            // CommsToolList
        }
        public bool GetIsGR()
        {
            return isGR;
            // Graphics
        }

        public bool GetIsActive()
        {
            return isActive;
        }

        public void ToggleActive()
        {
            isActive = !isActive;
            Console.WriteLine(modName + " set to " + isActive + ".");
        }
    }

    class MapMod 
    { 
        // Map mod class
    }

    class Program
    {
        private string version = "0.1";
        private string config, gameDir, backupDir, vehicleDir, mapDir, configDir, noesisDir;
        private List<VehicleMod> vehicleMods = new List<VehicleMod>();
        private List<MapMod> mapMods = new List<MapMod>();
        private bool firstRun = false;

        static void Main(string[] args)
        {
            Program BPRmenu = new Program();
            BPRmenu.Startup();
        }

        private void Startup()
        {
            // Controls program startup processes
            CheckConfig();
            RefreshVehicleList();
            Menu();
        }

        private void Menu()
        {
            // User menu and start point
            string option;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("------------------");
                Console.WriteLine("| BPR Mod Config |");
                Console.WriteLine("------------------");
                Console.WriteLine("- By CoolguyTC\n\n\n");
                Console.WriteLine("What do you want to do:\n");
                Console.WriteLine("(P)resets");
                Console.WriteLine("(V)ehicle mod config.");
                Console.WriteLine("(M)ap mod config.");
                Console.WriteLine("(C)hange directories.");
                Console.WriteLine("(H)elp.");
                //Console.WriteLine("(D)EBUG MODE");
                Console.WriteLine("(E)xit.");

                option = Console.ReadLine().ToUpper();

                switch (option)
                {
                    case "P":
                        Console.Clear();
                        Console.WriteLine("This is not in the " + version + " version!");
                        Thread.Sleep(4000);
                        break;
                    case "V":
                        ToggleVehicleMods();
                        break;
                    case "M":
                        Console.Clear();
                        Console.WriteLine("This is not in the " + version + " version!");
                        Thread.Sleep(4000);
                        break;
                    case "C":
                        ChangeDirectories();
                        break;
                    case "H":
                        HelpMenu();
                        break;
                    case "D":
                        //DebugOptions();
                        break;
                    case "E":
                        return;
                    default:
                        break;
                }
            }
        }

        private void HelpMenu()
        {
            // Help menu
            Console.Clear();
            Console.WriteLine("For help and info see the github page's README file.");
            Console.WriteLine("There you can get help and see the project's source code.");
            Console.WriteLine("Github page: https://github.com/CoolguyTC/BPR-Mod-Config");
            Console.WriteLine("\n[ENTER]");
            Console.ReadLine();
        }

        private void ToggleVehicleMods()
        {
            // Allows the user to toggle vehicle mods on and off
            string option;
            while (true)
            {
                Console.Clear();

                // Vehicle table creator
                var vehicleTable = new List<List<object>>();
                string enabled;
                vehicleTable.Add(new List<object> { "No.", "Enabled", "Car Name", "Replaced Car", "Replaces Attributes", "Replaces CommsToolList", "Replaces Graphics" });
                for (int i = 0; i < vehicleMods.Count; i++)
                {
                    if (vehicleMods[i].GetIsActive()) enabled = "[X]";
                    else enabled = "[ ]";
                    vehicleTable.Add(new List<object> { "(" + (i + 1) + ")", enabled, vehicleMods[i].GetName(), vehicleMods[i].GetReplacedCar(), "[" + vehicleMods[i].GetIsAT() + "]", "[" + vehicleMods[i].GetIsCD() + "]", "[" + vehicleMods[i].GetIsGR() + "]" });
                }
                ConsoleTableBuilder.From(vehicleTable).ExportAndWriteLine();
                // ---------------------
                Console.WriteLine("[" + CheckVehicleConfigActive() + "]");
                Console.WriteLine("\n(" + 1 + "-" + vehicleMods.Count  + ") Toggle a mod.");
                Console.WriteLine("(E)nable all.");
                Console.WriteLine("(D)isable all.");
                Console.WriteLine("(C)ompletely apply your current configuration. [Overwrites entire vehicle folder]");
                Console.WriteLine("(F)ast apply your current configuration.       [Skips unnecessary files]");
                Console.WriteLine("(B)ack.");
                option = Console.ReadLine().ToUpper();

                try
                {
                    vehicleMods[Convert.ToInt32(option) - 1].ToggleActive();
                    // Toggles each mod if a number is entered
                }
                catch
                {
                    switch (option)
                    {
                        case "E":
                            Console.Clear();
                            foreach (VehicleMod car in vehicleMods)
                            {
                                if (car.GetIsActive() == false)
                                {
                                    car.ToggleActive();
                                }
                            }
                            break;
                        case "D":
                            Console.Clear();
                            foreach (VehicleMod car in vehicleMods)
                            {
                                if (car.GetIsActive() == true)
                                {
                                    car.ToggleActive();
                                }
                            }
                            break;
                        case "C":
                            Console.Clear();
                            Console.WriteLine("Completely applying changes please wait...");
                            ApplyVehicleChanges();
                            return;
                        case "F":
                            Console.Clear();
                            Console.WriteLine("Quickly applying changes please wait...");
                            FastApplyVehicleChanges();
                            break;
                        case "B":
                            while (true)
                            {
                                Console.WriteLine("\nAre you sure you want to go back? (Y/N)");
                                Console.WriteLine("The changes HAVE NOT been applied yet!");
                                option = Console.ReadLine().ToUpper();
                                if (option == "Y")
                                {
                                    return;
                                }
                                else if (option == "N")
                                {
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
        }

        private string CheckVehicleConfigActive()
        {
            // Checks if the internal config and external one match
            bool isApplied;

            foreach (VehicleMod vehicle in vehicleMods)
            {
                isApplied = false;
                foreach (string car in config.Replace("\n", "").Split('#')[1].Split(';'))
                {
                    if (car != "CAR" && car != "")
                    {
                        if (car.Split(',')[0] == vehicle.GetName() && Convert.ToBoolean(car.Split(',')[2]) == vehicle.GetIsActive())
                        {
                            isApplied = true;
                        }
                    }
                }
                if (isApplied == false)
                {
                    return "NOT APPLIED";
                }
            }
            return "APPLIED";
        }

        private void DebugOptions()
        {
            // Debug menu to force certain functions
            string option;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("DEBUG MODE:");
                Console.WriteLine("(R)efresh vehicle list.");
                Console.WriteLine("(A)pply vehicle changes.");
                Console.WriteLine("(V)anilla.");
                Console.WriteLine("(B)ack.");

                option = Console.ReadLine().ToUpper();

                switch (option)
                {
                    case "R":
                        RefreshVehicleList();
                        break;
                    case "A":
                        ApplyVehicleChanges();
                        break;
                    case "V":
                        ToVanilla();
                        break;
                    case "B":
                        return;
                    default:
                        break;
                }
            }
        }

        private void RefreshVehicleList()
        {
            // Refreshes the internal vehicle list based on the vehicle directory
            string tempName;
            bool carsChanged;
            vehicleMods.Clear();
            string[] vehicleFiles = Directory.GetDirectories(vehicleDir);
            foreach (string file in vehicleFiles)
            {
                tempName = file.Substring(file.LastIndexOf('\\') + 1);
                foreach (string car in config.Replace("\n", "").Split('#')[1].Split(';'))
                {
                    if (car != "CAR" && car != "")
                    {
                        // Goes through each line in the config CAR section
                        if (car.Split(',')[0] == tempName)
                        {
                            // Checks if the mod already has an entry and refreshes it with the same active state.
                            vehicleMods.Add(new VehicleMod(tempName, vehicleDir, Convert.ToBoolean(car.Split(',')[2].ToLower())));
                            tempName = "";
                            break;
                        }
                    }
                }
                if (tempName != "")
                {
                    // Creates a new entry if the mod doesn't exist
                    vehicleMods.Add(new VehicleMod(tempName, vehicleDir, false));
                }
            }

            if (firstRun == false)
            {
                // Checks if vehicle files have changed since last run
                foreach (string file in vehicleFiles)
                {
                    // Checks if they have been added
                    tempName = file.Substring(file.LastIndexOf('\\') + 1);
                    carsChanged = true;
                    foreach (string car in config.Replace("\n", "").Split('#')[1].Split(';'))
                    {
                        if (car != "CAR" && car != "")
                        {
                            if (car.Split(',')[0] == tempName)
                            {
                                carsChanged = false;
                            }
                        }
                    }
                    if (carsChanged == true)
                    {
                        VehicleFilesChanged();
                        return;
                    }
                }
                foreach (string car in config.Replace("\n", "").Split('#')[1].Split(';'))
                {
                    // Checks if they have been removed
                    if (car != "CAR" && car != "")
                    {
                        carsChanged = true;
                        foreach (string file in vehicleFiles)
                        {
                            tempName = file.Substring(file.LastIndexOf('\\') + 1);
                            if (car.Split(',')[0] == tempName)
                            {
                                carsChanged = false;
                            }
                        }
                        if (carsChanged == true)
                        {
                            VehicleFilesChanged();
                            return;
                        }
                    }
                }
            }
        }

        private void VehicleFilesChanged()
        {
            // Triggers when vehicle files have been altered while the program is closed
            string option;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Vehicle files have been changed since the program last ran!");
                Console.WriteLine("Do you want to apply changes. [RECOMMENDED] (Y/N)");
                Console.WriteLine("This will remove deleted vehicle mods from the game and update the mod list.");
                option = Console.ReadLine().ToUpper();
                switch (option)
                {
                    case "Y":
                        Console.Clear();
                        Console.WriteLine("Applying changes please wait...");
                        ApplyVehicleChanges();
                        break;
                    case "N":
                        Console.Clear();
                        Console.WriteLine("Please be aware the vehicle config may now be innacurate until a complete apply is performed!");
                        Thread.Sleep(4000);
                        break;
                }
                break;
            }
        }

        private void FastApplyVehicleChanges()
        {
            // Only copies the original files for mods that have been changed to false since the last apply
            string tempData;
            string[] modFiles;

            List<string> directories = new List<string>();
            foreach (string car in config.Replace("\n", "").Split('#')[1].Split(';'))
            {
                // Stored a list of original file directories for mod files currently installed
                if (car != "CAR" && car != "")
                {
                    if (car.Split(',')[2] == "True")
                    {
                        foreach (string directory in Directory.GetFiles(vehicleDir + "\\" + car.Split(',')[0], "*.BIN"))
                        {
                            directories.Add(backupDir + "\\VEHICLES\\" + directory.Substring(directory.LastIndexOf("\\") + 1));
                        }
                    }
                }
            }

            foreach (string file in directories)
            {
                // Sets the saved vehicle files to default
                File.Copy(file, gameDir + "\\VEHICLES\\" + file.Substring(file.LastIndexOf("\\") + 1), true);
            }

            foreach (VehicleMod car in vehicleMods)
            {
                // Copies in neccessary modded vehicle files
                if (car.GetIsActive())
                {
                    modFiles = Directory.GetFiles(vehicleDir + "\\" + car.GetName(), "*.BIN");
                    foreach (string file in modFiles)
                    {
                        File.Copy(file, gameDir + "\\VEHICLES\\" + file.Substring(file.LastIndexOf("\\") + 1), true);
                    }
                }
            }

            config = config.Split('#')[0] + "#\nCAR;\n#" + config.Split('#')[2];
            foreach (VehicleMod car in vehicleMods)
            {
                // Updates the config file with the new changes
                tempData = "\n" + car.GetName() + "," + car.GetReplacedCar() + "," + car.GetIsActive() + ";";
                config = config.Insert(GetIndexes(config, ';')[5], tempData);
            }
            File.WriteAllText(configDir, config);
            Console.WriteLine("Changes applied to: " + configDir);
        }

        private void ApplyVehicleChanges()
        {
            // Copies over the entire vehicle folder
            string tempData;
            string[] originalCars = Directory.GetFiles(backupDir + "\\VEHICLES", "*.BIN");
            string[] modFiles;

            foreach (string file in originalCars)
            {
                // Sets all vehicle files to default
                if (file.Substring(file.LastIndexOf("\\") + 1) != "VEHICLETEX.BIN")
                {
                    File.Copy(file, gameDir + "\\VEHICLES\\" + file.Substring(file.LastIndexOf("\\") + 1), true);
                }
            }

            foreach (VehicleMod car in vehicleMods)
            {
                // Copies in neccessary modded vehicle files
                if (car.GetIsActive())
                {
                    modFiles = Directory.GetFiles(vehicleDir + "\\" + car.GetName(), "*.BIN");
                    foreach (string file in modFiles)
                    {
                        File.Copy(file, gameDir + "\\VEHICLES\\" + file.Substring(file.LastIndexOf("\\") + 1), true);
                    }
                }
            }

            config = config.Split('#')[0] + "#\nCAR;\n#" + config.Split('#')[2];
            foreach (VehicleMod car in vehicleMods)
            {
                // Updates the config file with the new changes
                tempData = "\n" + car.GetName() + "," + car.GetReplacedCar() + "," + car.GetIsActive() + ";";
                config = config.Insert(GetIndexes(config, ';')[5], tempData);
            }
            File.WriteAllText(configDir, config);
            Console.WriteLine("Changes applied to: " + configDir);
        }

        private void ToVanilla()
        {
            // Sets all vehicle and (soon) map files to default
            foreach (VehicleMod car in vehicleMods)
            {
                if (car.GetIsActive())
                {
                    car.ToggleActive();
                }
            }
            ApplyVehicleChanges();
        }

        private void CheckConfig()
        {
            // Checks the config file on startup
            configDir = Path.Combine(Convert.ToString(Directory.GetCurrentDirectory()), "DATA\\config.ini");
            if (File.Exists(configDir) == false)
            {
                // Generates an empty config file if one is not found
                Console.WriteLine("Config file not found! Generating...");
                File.WriteAllText(configDir, "Game Directory;\nBackup Directory;\nModded Vehicle Directory;\nModded Map Directory;\nNoesis Directory;\n#\nCAR;\n#\nMAP;");
                config = File.ReadAllText(configDir);
                Console.WriteLine("Blank config file generated as: " + configDir);
                Console.WriteLine("\nMissing directories found!\n");
                AssignDirectories();
                return;
            }
            config = File.ReadAllText(configDir);

            for (int i = 0; i < 5; i++)
            {
                // Checks each directory for missing entries
                if (config.Split('\n')[i].Split(';')[1] == "")
                {
                    Console.WriteLine("\nMissing directories found!\n");
                    AssignDirectories();
                    return;
                }
            }
            // Sets all internal directory variables from the config file
            gameDir = config.Split('\n')[0].Split(';')[1];
            backupDir = config.Split('\n')[1].Split(';')[1];
            vehicleDir = config.Split('\n')[2].Split(';')[1];
            mapDir = config.Split('\n')[3].Split(';')[1];
            noesisDir = config.Split('\n')[4].Split(';')[1];
        }

        private void AssignDirectories()
        {
            // Allows the user to enter missing directories
            string option;
            int counter = 0;
            if (config.Split('\n')[0].Split(';')[1] == "")
            {
                Console.WriteLine("Please paste the directory of Burnout Paradise Remastered:");
                gameDir = Console.ReadLine();
                config = config.Insert(GetIndexes(config, ';')[0], gameDir);
                counter++;
                Console.WriteLine("");
            }
            else
            {
                gameDir = config.Split('\n')[0].Split(';')[1];
            }
            if (config.Split('\n')[1].Split(';')[1] == "")
            {
                Console.WriteLine("Please paste the directory of your Burnout Paradise Remastered backup folder:");
                backupDir = Console.ReadLine();
                config = config.Insert(GetIndexes(config, ';')[1], backupDir);
                counter++;
                Console.WriteLine("");
            }
            else
            {
                backupDir = config.Split('\n')[1].Split(';')[1];
            }
            if (config.Split('\n')[2].Split(';')[1] == "")
            {
                Console.WriteLine("Please paste the directory of your vehicle mods folder:");
                vehicleDir = Console.ReadLine();
                config = config.Insert(GetIndexes(config, ';')[2], vehicleDir);
                counter++;
                Console.WriteLine("");
            }
            else
            {
                vehicleDir = config.Split('\n')[2].Split(';')[1];
            }
            if (config.Split('\n')[3].Split(';')[1] == "")
            {
                Console.WriteLine("Please paste the directory of your map mods folder:");
                mapDir = Console.ReadLine();
                config = config.Insert(GetIndexes(config, ';')[3], mapDir);
                counter++;
                Console.WriteLine("");
            }
            else
            {
                mapDir = config.Split('\n')[3].Split(';')[1];
            }
            if (config.Split('\n')[4].Split(';')[1] == "")
            {
                Console.WriteLine("Please paste the directory of your noesis folder:");
                noesisDir = Console.ReadLine();
                config = config.Insert(GetIndexes(config, ';')[4], noesisDir);
                counter++;
                Console.WriteLine("");
            }
            else
            {
                noesisDir = config.Split('\n')[4].Split(';')[1];
            }

            File.WriteAllText(configDir, config);
            Console.WriteLine("Directories saved to: " + configDir);
            Thread.Sleep(4000);

            if (counter == 5)
            {
                // First run procedure
                firstRun = true;
                while (true)
                {
                    Console.WriteLine("\n----------\nDISCLAMER!");
                    Console.WriteLine("Is this your first time running this application?");
                    Console.WriteLine("If so, it is recommended that you reset your vehicle files before starting, as the program is unable to detect what mods you currently have running.");
                    Console.WriteLine("Do you want to reset Burnout Paradise Remastered's vehicle files to default? (Y/N) [This WILL NOT effect any other folders in the game's directory]");
                    option = Console.ReadLine().ToUpper();
                    if (option == "Y")
                    {
                        Console.Clear();
                        Console.WriteLine("Reseting vehicle files please wait...");
                        RefreshVehicleList();
                        ToVanilla();
                        break;
                    }
                    else if (option == "N")
                    {
                        Console.Clear();
                        Console.WriteLine("Vehicle files not reset!\n");
                        Thread.Sleep(4000);
                        break;
                    }
                }
            }
        }

        private void ChangeDirectories()
        {
            // Allows the user to change selected directories
            string option;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Which directory do you want to change:\n");

                var optionTable = new List<List<object>>();
                optionTable.Add(new List<object> { "Option", "Current Directory" });
                optionTable.Add(new List<object> { "(G)ame directory.", gameDir });
                optionTable.Add(new List<object> { "Back(U)p directory.", backupDir });
                optionTable.Add(new List<object> { "(V)ehicle mods directory.", vehicleDir });
                optionTable.Add(new List<object> { "(M)ap mods directory.", mapDir });
                optionTable.Add(new List<object> { "(N)oesis directory.", noesisDir });
                ConsoleTableBuilder.From(optionTable).ExportAndWriteLine();
                Console.WriteLine("  (B)ack.");

                option = Console.ReadLine().ToUpper();
                Console.WriteLine();

                switch (option)
                {
                    case "G":
                        config = config.Replace(gameDir, "");
                        AssignDirectories();
                        break;
                    case "U":
                        config = config.Replace(backupDir, "");
                        AssignDirectories();
                        break;
                    case "V":
                        config = config.Replace(vehicleDir, "");
                        AssignDirectories();
                        break;
                    case "M":
                        config = config.Replace(mapDir, "");
                        AssignDirectories();
                        break;
                    case "N":
                        config = config.Replace(noesisDir, "");
                        AssignDirectories();
                        break;
                    case "B":
                        return;
                    default:
                        break;
                }
            }
        }

        private int[] GetIndexes(string text, char character)
        {
            // Gets every index of a specified character in a string
            List<int> indexes = new List<int>();
            int i = 0, cumulativeValue = 0, tempValue = 0;

            while (true)
            {
                if (text.IndexOf(character) == -1) break;
                tempValue = text.IndexOf(character);
                indexes.Add(tempValue + cumulativeValue + 1);
                cumulativeValue = indexes[i];
                text = text.Substring(tempValue + 1);
                i++;
            }

            return indexes.ToArray();
        }
    }
}