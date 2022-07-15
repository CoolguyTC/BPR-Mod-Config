using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;

namespace BPR_Mod_Manager
{
    class VehicleMod 
    {
        private string modName, replacedCarID;
        private bool isAT, isCD, isGR, isActive;

        public VehicleMod(string fileName, string directory, bool active)
        {
            modName = fileName;
            string tempPath = Directory.GetFiles(directory + "\\" + fileName, "*.BIN")[0];
            replacedCarID = tempPath.Substring(tempPath.LastIndexOf('\\') + 1).Split('_')[1];
            isActive = active;
            if (File.Exists(directory + "/" + fileName + "/VEH_" + replacedCarID + "_AT.BIN")) isAT = true;
            else isAT = false;
            if (File.Exists(directory + "/" + fileName + "/VEH_" + replacedCarID + "_CD.BIN")) isCD = true;
            else isCD = false;
            if (File.Exists(directory + "/" + fileName + "/VEH_" + replacedCarID + "_GR.BIN")) isGR = true;
            else isGR = false;
        }

        public string GetName()
        {
            return modName;
        }

        public string GetReplacedCar()
        {
            return replacedCarID;
            // This will be fed through the spreadsheet and converted to car name.
        }

        public string GetReplaces()
        {
            string text = "";
            if (isAT) text += " X";
            if (isCD) text += " Y";
            if (isGR) text += " Z";
            return text;
        }

        public bool getIsActive()
        {
            return isActive;
        }

        public void toggleActive()
        {
            isActive = !isActive;
            Console.WriteLine(modName + " set to " + isActive + ".");
        }
    }

    class MapMod 
    { 

    }

    class Program
    {
        public string version = "beta";
        public string config, gameDir, backupDir, vehicleDir, mapDir, configDir;
        public List<VehicleMod> vehicleMods = new List<VehicleMod>();
        public List<MapMod> mapMods;

        static void Main(string[] args)
        {
            Program BPRmenu = new Program();
            BPRmenu.menu();
        }

        public void menu()
        {
            checkConfig();
            refreshVehicleList();
            options();
        }

        public void options()
        {
            string option;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("------------------");
                Console.WriteLine("| BPR Mod Config |");
                Console.WriteLine("------------------");
                Console.WriteLine("By CoolguyTC\n\n\n");
                Console.WriteLine("What do you want to do:\n");
                Console.WriteLine("(V)ehicle mod config.");
                Console.WriteLine("(M)ap mod config.");
                Console.WriteLine("(H)elp.");
                Console.WriteLine("(D)EBUG MODE");
                Console.WriteLine("(E)xit.");

                option = Console.ReadLine().ToUpper();

                switch (option)
                {
                    case "V":
                        toggleVehicleMods();
                        break;
                    case "M":
                        Console.Clear();
                        Console.WriteLine("This is not in the " + version + " version!");
                        Thread.Sleep(4000);
                        break;
                    case "H":
                        helpMenu();
                        break;
                    case "D":
                        debugOptions();
                        break;
                    case "E":
                        return;
                    default:
                        break;
                }
            }
        }

        public void helpMenu()
        {
            string option;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("What do you need help with?");
                Console.WriteLine("(O)ther.");
                Console.WriteLine("(B)ack.");
                option = Console.ReadLine().ToUpper();
                switch (option)
                {
                    case "O":
                        Console.Clear();
                        Console.WriteLine("For any further questions you can @ me on the Burnout Hints discord or DM me.");
                        Console.WriteLine("My discord is 'ThomasMLGEngine#3576'");
                        Console.WriteLine("\n[ENTER]");
                        Console.ReadLine();
                        break;
                    case "B":
                        return;
                }
            }
        }

        public void toggleVehicleMods()
        {
            string option;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-------------------------------------------------------------");
                for (int i = 0; i < vehicleMods.Count; i++)
                {
                    Console.WriteLine("(" + (i + 1) + ") " + vehicleMods[i].GetName() + " [" + vehicleMods[i].getIsActive() + "] Replaces: " + vehicleMods[i].GetReplacedCar() + ".");
                }
                Console.WriteLine("-------------------------------------------------------------\n");
                Console.WriteLine("\n(" + 1 + "-" + vehicleMods.Count  + ") Toggle a mod.");
                Console.WriteLine("(A)pply your current configuration.      [Overwrites entire vehicle folder]");
                Console.WriteLine("(F)ast apply your current configuration. [Skips unnecessary files]");
                Console.WriteLine("(B)ack.");
                option = Console.ReadLine().ToUpper();
                try
                {
                    vehicleMods[Convert.ToInt32(option) - 1].toggleActive();
                }
                catch
                {
                    switch (option)
                    {
                        case "A":
                            applyVehicleChanges();
                            return;
                        case "F":
                            fastApplyVehicleChanges();
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

        public void debugOptions()
        {
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
                        refreshVehicleList();
                        break;
                    case "A":
                        applyVehicleChanges();
                        break;
                    case "V":
                        toVanilla();
                        break;
                    case "B":
                        return;
                    default:
                        break;
                }
            }
        }

        public void refreshVehicleList()
        {
            string tempName;
            vehicleMods.Clear();
            string[] vehicleFiles = Directory.GetDirectories(vehicleDir);
            foreach (string file in vehicleFiles)
            {
                tempName = file.Substring(file.LastIndexOf('\\') + 1);
                foreach (string car in config.Replace("\n", "").Split('#')[1].Split(';'))
                {
                    if (car != "CAR")
                    {
                        if (car.Split(',')[0] == tempName)
                        {
                            vehicleMods.Add(new VehicleMod(tempName, vehicleDir, Convert.ToBoolean(car.Split(',')[2].ToLower())));
                            tempName = "";
                            break;
                        }
                    }
                }
                if (tempName != "")
                {
                    vehicleMods.Add(new VehicleMod(tempName, vehicleDir, false));
                }
            }
        }

        public void fastApplyVehicleChanges()
        {

        }

        public void applyVehicleChanges()
        {
            string tempData;
            string[] originalCars = Directory.GetFiles(backupDir + "\\VEHICLES", "*.BIN");
            string[] modFiles;

            foreach (string file in originalCars)
            {
                if (file.Substring(file.LastIndexOf("\\") + 1) != "VEHICLETEX.BIN")
                {
                    File.Copy(file, gameDir + "\\VEHICLES\\" + file.Substring(file.LastIndexOf("\\") + 1), true);
                    Console.WriteLine(file);
                }
            }

            foreach (VehicleMod car in vehicleMods)
            {
                if (car.getIsActive())
                {
                    modFiles = Directory.GetFiles(vehicleDir + "\\" + car.GetName(), "*.BIN");
                    foreach (string file in modFiles)
                    {
                        File.Copy(file, gameDir + "\\VEHICLES\\" + file.Substring(file.LastIndexOf("\\") + 1), true);
                        Console.WriteLine("MOD: " + file);
                    }
                }
            }

            config = config.Split('#')[0] + "#\nCAR;\n#" + config.Split('#')[2];

            foreach (VehicleMod car in vehicleMods)
            {
                tempData = "\n" + car.GetName() + "," + car.GetReplacedCar() + "," + car.getIsActive() + ";";
                config = config.Insert(getIndexes(config, ';')[4], tempData);
            }
            File.WriteAllText(configDir, config);
            Console.WriteLine("Changes applied to: " + configDir);
        }

        public void toVanilla()
        {
            foreach (VehicleMod car in vehicleMods)
            {
                if (car.getIsActive())
                {
                    car.toggleActive();
                }
            }
            applyVehicleChanges();
        }

        public void checkConfig()
        {
            configDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.ini");
            if (File.Exists(configDir) == false)
            {
                Console.WriteLine("Config file not found! Generating...");
                File.WriteAllText(configDir, "Game Directory;\nBackup Directory;\nModded Vehicle Directory;\nModded Map Directory;\n#\nCAR;\n#\nMAP;");
                config = File.ReadAllText(configDir);
                Console.WriteLine("Blank config file generated as: " + configDir);
                assignDirectories();
                return;
            }
            config = File.ReadAllText(configDir);

            if (config.Split('\n')[0].Split(';')[1] == "")
            {
                assignDirectories();
                return;
            }
            else
            {
                gameDir = config.Split('\n')[0].Split(';')[1];
            }

            if (config.Split('\n')[1].Split(';')[1] == "")
            {
                assignDirectories();
                return;
            }
            else
            {
                backupDir = config.Split('\n')[1].Split(';')[1];
            }

            if (config.Split('\n')[2].Split(';')[1] == "")
            {
                assignDirectories();
                return;
            }
            else
            {
                vehicleDir = config.Split('\n')[2].Split(';')[1];
            }

            if (config.Split('\n')[3].Split(';')[1] == "")
            {
                assignDirectories();
                return;
            }
            else
            {
                mapDir = config.Split('\n')[3].Split(';')[1];
            }

            Console.WriteLine("All directories located!");
        }

        public void assignDirectories()
        {
            string option;
            int counter = 0;
            Console.WriteLine("\nMissing directories found!\n");
            if (config.Split('\n')[0].Split(';')[1] == "")
            {
                Console.WriteLine("Please paste the directory of Burnout Paradise Remastered:");
                gameDir = Console.ReadLine();
                config = config.Insert(getIndexes(config, ';')[0], gameDir);
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
                config = config.Insert(getIndexes(config, ';')[1], backupDir);
                counter++;
                Console.WriteLine("");
            }
            else
            {
                backupDir = config.Split('\n')[1].Split(';')[1];
            }
            if (config.Split('\n')[2].Split(';')[1] == "")
            {
                Console.WriteLine("Please paste the directory of your vehicle mods:");
                vehicleDir = Console.ReadLine();
                config = config.Insert(getIndexes(config, ';')[2], vehicleDir);
                counter++;
                Console.WriteLine("");
            }
            else
            {
                vehicleDir = config.Split('\n')[2].Split(';')[1];
            }
            if (config.Split('\n')[3].Split(';')[1] == "")
            {
                Console.WriteLine("Please paste the directory of your map mods:");
                mapDir = Console.ReadLine();
                config = config.Insert(getIndexes(config, ';')[3], mapDir);
                counter++;
                Console.WriteLine("");
            }
            else
            {
                mapDir = config.Split('\n')[3].Split(';')[1];
            }
            File.WriteAllText(configDir, config);
            Console.WriteLine("Directories saved to: " + configDir);

            if (counter == 4)
            {
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
                        refreshVehicleList();
                        toVanilla();
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

        public int[] getIndexes(string text, char character)
        {
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
