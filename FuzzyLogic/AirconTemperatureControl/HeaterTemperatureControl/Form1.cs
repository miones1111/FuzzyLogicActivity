using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotFuzzy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HeaterTemperatureControl
{
    public partial class Form1 : Form
    {
        FuzzyEngine fe;
        MembershipFunctionCollection initialTemp, desiredTemp, actionOutput;
        LinguisticVariable myInitialTemp, myDesiredTemp, myActionOutput;
        FuzzyRuleCollection myrules;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setMembers();
            setRules();
            setFuzzyEngine();
        }

        public void setMembers()
        {
            initialTemp = new MembershipFunctionCollection();
            initialTemp.Add(new MembershipFunction("TOO_COLD", 16.0, 18.0, 18.5, 19.0));
            initialTemp.Add(new MembershipFunction("COLD", 20.0,20.5, 21.0, 21.5));
            initialTemp.Add(new MembershipFunction("WARM", 22.0, 22.5, 23.0, 23.5));
            initialTemp.Add(new MembershipFunction("HOT", 28.0, 29.0, 30.0, 31.0));
            initialTemp.Add(new MembershipFunction("TOO_HOT", 31.0, 31.0, 31.0, 31.0));
            myInitialTemp = new LinguisticVariable("INITIAL_TEMP", initialTemp);

            desiredTemp = new MembershipFunctionCollection();
            desiredTemp.Add(new MembershipFunction("TOO_COLD", 16.0, 18.0, 18.5, 19.0));
            desiredTemp.Add(new MembershipFunction("COLD", 20.0, 20.5, 21.0, 21.5));
            desiredTemp.Add(new MembershipFunction("WARM", 22.0, 22.5, 23.0, 23.5));
            desiredTemp.Add(new MembershipFunction("HOT", 28.0, 29.0, 30.0, 31.0));
            desiredTemp.Add(new MembershipFunction("TOO_HOT", 31.0, 31.0, 31.0, 31.0));
            myDesiredTemp = new LinguisticVariable("DESIRED_TEMP", desiredTemp);

            actionOutput = new MembershipFunctionCollection();
            actionOutput.Add(new MembershipFunction("DOWN", 16.0, 18.0, 18.0, 19.0));
            actionOutput.Add(new MembershipFunction("NO_ACTION", 21.5, 20.5, 20.5, 21.5));
            actionOutput.Add(new MembershipFunction("UP", 16.0, 18.5, 18.5, 23.0));
            myActionOutput = new LinguisticVariable("ACTION_OUTPUT", actionOutput);
        }

        public void setRules()
        {
            myrules = new FuzzyRuleCollection();

            // If Initial Temperature is TOO_COLD
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS TOO_COLD) AND (DESIRED_TEMP IS TOO_COLD) THEN ACTION_OUTPUT IS NO_ACTION"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS TOO_COLD) AND (DESIRED_TEMP IS COLD) THEN ACTION_OUTPUT IS UP"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS TOO_COLD) AND (DESIRED_TEMP IS WARM) THEN ACTION_OUTPUT IS UP"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS TOO_COLD) AND (DESIRED_TEMP IS HOT) THEN ACTION_OUTPUT IS UP"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS TOO_COLD) AND (DESIRED_TEMP IS TOO_HOT) THEN ACTION_OUTPUT IS UP"));

            // If Initial Temperature is COLD
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS COLD) AND (DESIRED_TEMP IS TOO_COLD) THEN ACTION_OUTPUT IS DOWN"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS COLD) AND (DESIRED_TEMP IS COLD) THEN ACTION_OUTPUT IS NO_ACTION"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS COLD) AND (DESIRED_TEMP IS WARM) THEN ACTION_OUTPUT IS UP"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS COLD) AND (DESIRED_TEMP IS HOT) THEN ACTION_OUTPUT IS UP"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS COLD) AND (DESIRED_TEMP IS TOO_HOT) THEN ACTION_OUTPUT IS UP"));

            // If Initial Temperature is WARM
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS WARM) AND (DESIRED_TEMP IS TOO_COLD) THEN ACTION_OUTPUT IS DOWN"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS WARM) AND (DESIRED_TEMP IS COLD) THEN ACTION_OUTPUT IS DOWN"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS WARM) AND (DESIRED_TEMP IS WARM) THEN ACTION_OUTPUT IS NO_ACTION"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS WARM) AND (DESIRED_TEMP IS HOT) THEN ACTION_OUTPUT IS UP"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS WARM) AND (DESIRED_TEMP IS TOO_HOT) THEN ACTION_OUTPUT IS UP"));

            // If Initial Temperature is HOT
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS HOT) AND (DESIRED_TEMP IS TOO_COLD) THEN ACTION_OUTPUT IS DOWN"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS HOT) AND (DESIRED_TEMP IS COLD) THEN ACTION_OUTPUT IS DOWN"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS HOT) AND (DESIRED_TEMP IS WARM) THEN ACTION_OUTPUT IS DOWN"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS HOT) AND (DESIRED_TEMP IS HOT) THEN ACTION_OUTPUT IS NO_ACTION"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS HOT) AND (DESIRED_TEMP IS TOO_HOT) THEN ACTION_OUTPUT IS UP"));

            // If Initial Temperature is TOO_HOT
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS TOO_HOT) AND (DESIRED_TEMP IS TOO_COLD) THEN ACTION_OUTPUT IS DOWN"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS TOO_HOT) AND (DESIRED_TEMP IS COLD) THEN ACTION_OUTPUT IS DOWN"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS TOO_HOT) AND (DESIRED_TEMP IS WARM) THEN ACTION_OUTPUT IS DOWN"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS TOO_HOT) AND (DESIRED_TEMP IS HOT) THEN ACTION_OUTPUT IS DOWN"));
            myrules.Add(new FuzzyRule("IF (INITIAL_TEMP IS TOO_HOT) AND (DESIRED_TEMP IS TOO_HOT) THEN ACTION_OUTPUT IS NO_ACTION"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fuziffyvalues();
            defuzzy();
            computeRoomTemp();
        }

        public void setFuzzyEngine()
        {
            fe = new FuzzyEngine();
            fe.LinguisticVariableCollection.Add(myInitialTemp);
            fe.LinguisticVariableCollection.Add(myDesiredTemp);
            fe.LinguisticVariableCollection.Add(myActionOutput);
            fe.FuzzyRuleCollection = myrules;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            initialTempLbl.Text = "";
            desiredTempLbl.Text = "";
            roomTempLbl.Text = "";
            actionOutputLbl.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            setLabels();
            roomTempLbl.Text = textBox1.Text;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        public void fuziffyvalues()
        {
            myInitialTemp.InputValue = (Convert.ToDouble(roomTempLbl.Text));
            myInitialTemp.Fuzzify("WARM");
            myDesiredTemp.InputValue = (Convert.ToDouble(textBox2.Text));
            myDesiredTemp.Fuzzify("WARM");

        }
        public void defuzzy()
        {
            fe.Consequent = "ACTION_OUTPUT";
            actionOutputLbl.Text = "" + fe.Defuzzify();
        }

        public void computeRoomTemp()
        {
            double oldRoomTemp = Convert.ToDouble(roomTempLbl.Text);
            double oldActionOutput = Convert.ToDouble(actionOutputLbl.Text);
            double newCurrentRoomTemp = oldRoomTemp + oldActionOutput;
            roomTempLbl.Text = newCurrentRoomTemp.ToString();
        }
        public void setLabels()
        {
            initialTempLbl.Text = textBox1.Text;
            desiredTempLbl.Text = textBox2.Text;
        }
    }
}
