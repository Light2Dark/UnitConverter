using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace DailyCalculator.Droid
{
    [Activity(Label = "DailyCalculator", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        EditText inputText, outputText;
        Button convertButton;
        Spinner inputSpinner, outputSpinner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            inputText = FindViewById<EditText>(Resource.Id.inputText);
            outputText = FindViewById<EditText>(Resource.Id.outputText);
            convertButton = FindViewById<Button>(Resource.Id.buttonConvert);
            inputSpinner = FindViewById<Spinner>(Resource.Id.inputUnit);
            outputSpinner = FindViewById<Spinner>(Resource.Id.outputUnit);
            float inputNum = 0;
            float conversionConstant = 0;

            // input code
            inputText.AfterTextChanged += delegate { float.TryParse(inputText.Text, out inputNum); };

            // Conversion code
            convertButton.Click += delegate
            {
                // if user does nothing & presses convert button
                if (inputSpinner.SelectedItem.ToString() == "kilogram" && outputSpinner.SelectedItem.ToString() == "kilogram")
                {
                    conversionConstant = 0;
                }
                conversionConstant = calculateConversionConstant(inputSpinner, outputSpinner);
                if (conversionConstant == 0)
                {
                    throw new InvalidOperationException("You have chosen a wrong combination. Please try again!");
                }

                // output code
                float result = convertNum(inputNum, conversionConstant, inputSpinner, outputSpinner);
                outputText.Text = result.ToString();
            };

            // One solution: Make input unit = 1 by default. Depending on case, conversionConstant can be determined from that
            // if (inputUnit is kg and outputUnit is oz, then c = 2.5). Can make this with switch case
            // OR. Another solution would be to use a base unit (like kg) and convert inputUnit based on that
            // If (inputUnit is oz, then c = oz/kg)...
            // Going with 1st solution, second one doesnt make sense & doesnt work with different base units (K,secs..)
        }

        private float calculateConversionConstant (Spinner input, Spinner output)
        {
            float cc = 0;

            if (inputSpinner.SelectedItem.ToString() == "kilogram" && outputSpinner.SelectedItem.ToString() == "pounds(lbs)")
                cc = 2.2f;
            else if (inputSpinner.SelectedItem.ToString() == "pounds(lbs)" && outputSpinner.SelectedItem.ToString() == "kilogram")
                cc = 0.45f;
            else if (inputSpinner.SelectedItem.ToString() == "inches" && outputSpinner.SelectedItem.ToString() == "centimetre")
                cc = 2.54f;
            else if (inputSpinner.SelectedItem.ToString() == "centimetre" && outputSpinner.SelectedItem.ToString() == "inches")
                cc = 0.39f;
            else if (inputSpinner.SelectedItem.ToString() == "centimetre" && outputSpinner.SelectedItem.ToString() == "feet")
                cc = 0.03f;
            else if (inputSpinner.SelectedItem.ToString() == "feet" && outputSpinner.SelectedItem.ToString() == "centimetre")
                cc = 30.48f;
            else if (inputSpinner.SelectedItem.ToString() == "feet" && outputSpinner.SelectedItem.ToString() == "inches")
                cc = 12f;
            else if (inputSpinner.SelectedItem.ToString() == "celsius" && outputSpinner.SelectedItem.ToString() == "fahrenheit")
                cc = 1.8f;
            else if (inputSpinner.SelectedItem.ToString() == "fahrenheit" && outputSpinner.SelectedItem.ToString() == "celsius")
                cc = 1.8f;
            else
                cc = 0;

            return cc;
        }

        private float convertNum(float inputNum, float conversionConstant, Spinner input, Spinner output)
        {
            if (conversionConstant == 1.8f)
            {
                if (input.SelectedItem.ToString() == "celsius" && output.SelectedItem.ToString() == "fahrenheit")
                    return inputNum * conversionConstant + 32.0f;
                if (input.SelectedItem.ToString() == "fahrenheit" && output.SelectedItem.ToString() == "celsius")
                    return (inputNum - 32.0f) / 1.8f;
            }
            float result = inputNum * conversionConstant;
            return result;
        }
    }
}

