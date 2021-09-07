using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;


namespace Phantasm_Parser
{
    public partial class frmMain : Form
    {
        
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel._Worksheet oSTSheet;
            int[] slotRows = new int[6];
            List<StoryTeller> storyTellerList = new List<StoryTeller>();
            List<Day> days = new List<Day>();
            Schedule schedule = new Schedule();
            Day day = new Day();
            Slot slot = new Slot();

            openFile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                //Start Excel and get Application object.
                oXL = new Excel.Application();
                oXL.Visible = false;

                oWB = oXL.Workbooks.Open(openFile.FileName);
                oSheet = oWB.Sheets["RPG Website"];
                oSTSheet = oWB.Sheets["STList"];

                int lastUsedRow = 300;
                int slotCtr = 0;
                string cellValue;
                string gameST;
                Regex reg = new Regex("[,\\.]");
                int storyTellerId;
                double progress = 0;


                for (int rowCtr = 1; rowCtr < lastUsedRow; rowCtr++)
                {
                    if (oSTSheet.Cells[rowCtr, 1].Value != null)
                    {
                        gameST = (string)(oSTSheet.Cells[rowCtr, 1]).Value;

                        StoryTeller st = new StoryTeller();
                        st.Id = rowCtr;
                        st.Name = reg.Replace(gameST, string.Empty);
                        if ((oSTSheet.Cells[rowCtr, 3]).Value == null)
                        {
                            st.Description = string.Empty;
                        } else
                        {
                            st.Description = (string)(oSTSheet.Cells[rowCtr, 3]).Value;
                        }
                        storyTellerList.Add(st);
                    }
                    progress = (((double)rowCtr / (double)lastUsedRow) * 100) / 3;
                    Console.WriteLine($"row: {rowCtr}; lastRow: {lastUsedRow}; progress: {progress}");
                    progressBar1.Value = Convert.ToInt32(progress);
                }

                for (int rowCtr = 1; rowCtr < lastUsedRow; rowCtr++)
                {
                    cellValue = (string)(oSheet.Cells[rowCtr, 1]).Value;
                    if (cellValue == "Friday" || cellValue == "Saturday" || cellValue == "Sunday")
                    {                        
                        if (days.Find(s => s.Weekday == cellValue) == null)
                        {
                            day = new Day();
                            switch (cellValue)
                            {
                                case "Friday":
                                    day.Id = 1;
                                    break;
                                case "Saturday":
                                    day.Id = 2;
                                    break;
                                case "Sunday":
                                    day.Id = 3;
                                    break;
                            }                           
                            day.Weekday = cellValue;
                            day.Slots = new List<Slot>();
                            slot = new Slot();
                            slot.Id = slotCtr;
                            slot.name = oSheet.Cells[rowCtr, 2].value;
                            day.Slots.Add(slot);
                            days.Add(day);                        
                        } else
                        {
                            day = days.Find(s => s.Weekday == cellValue);
                            slot = new Slot();
                            slot.Id = slotCtr;
                            slot.name = oSheet.Cells[rowCtr, 2].value;
                            day.Slots.Add(slot);
                        }
                        slotRows[slotCtr] = rowCtr;
                        slotCtr++;
                    }
                    progress = 100 + ((((double)rowCtr / (double)lastUsedRow) * 100) / 3);
                    progressBar1.Value = Convert.ToInt32(progress);
                }
                slotRows[slotCtr] = lastUsedRow;

                foreach(Day dayCtr in days)
                {
                    foreach(Slot theSlot in dayCtr.Slots)
                    {
                        for (int rowCtr = (slotRows[theSlot.Id] + 1); rowCtr < (slotRows[theSlot.Id + 1] - 1); rowCtr++)
                        {
                            if ((oSheet.Cells[rowCtr, 1]).Value != null)
                            {
                                gameST = (string)(oSheet.Cells[rowCtr, 2]).Value;
                                gameST = reg.Replace(gameST, string.Empty);

                                if (storyTellerList.Find(s => s.Name == gameST) == null)
                                {
                                    StoryTeller st = new StoryTeller();
                                    st.Id = storyTellerList.Max(s => s.Id) + 1;
                                    st.Name = gameST;
                                    st.Description = string.Empty;
                                    storyTellerList.Add(st);
                                    storyTellerId = st.Id;
                                }
                                else
                                {
                                    storyTellerId = storyTellerList.Find(s => s.Name == gameST).Id;
                                }

                                Game game = new Game();
                                game.Id = rowCtr;
                                game.Day = dayCtr.Id;
                                game.Slot = theSlot.Id;                         
                                game.GameType = (string)(oSheet.Cells[rowCtr, 1]).Value;
                                game.StoryTellerId = storyTellerId;
                                game.StoryTellerName = (string)(oSheet.Cells[rowCtr, 2]).Value;
                                if ((oSheet.Cells[rowCtr, 3]).Value == null) {
                                    game.Title = string.Empty;
                                } else {
                                    game.Title = (string)(oSheet.Cells[rowCtr, 3]).Value;
                                }
                                if ((oSheet.Cells[rowCtr, 4]).Value == null)
                                {
                                    game.Description = string.Empty;
                                } else
                                {
                                    game.Description = Regex.Replace((string)(oSheet.Cells[rowCtr, 4]).Value, "\n", "<br />");
                                }
                                theSlot.games.Add(game);
                            }
                            progress = 200 + ((((double)rowCtr / (double)lastUsedRow) * 100) / 3);
                            progressBar1.Value = Convert.ToInt32(progress);
                        }
                    }
                }
                
                schedule.days = days;
                schedule.storytellers = storyTellerList;

                oWB.Close();
                oXL.Quit();

                saveFile.Filter = "JSON|*.json";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {                   
                    JsonSerializer serializer = new JsonSerializer();
                    using (StreamWriter sw = new StreamWriter(saveFile.FileName))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, schedule);                  
                    }
                    MessageBox.Show("Done");
                }
                Application.Exit();
            }            
        }
    }
}
