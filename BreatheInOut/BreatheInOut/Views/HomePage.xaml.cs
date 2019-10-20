using BreatheInOut.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BreatheInOut.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        bool timerRunning = false;
        float animationProgress = 0.0f;

        public HomePage()
        {
            InitializeComponent();
        }

        private async void playButton_Clicked(object sender, EventArgs e)
        {
            playButton.IsVisible = false;

            if (!animationView.IsPlaying)
            {
                playButton.Source = "pause.png";
                timerRunning = true;
                animationView.IsPlaying = true;
                progressSlider.IsEnabled = true;
                StartTimer();
            }
            else
            {
                playButton.Source = "play.png";
                timerRunning = false;
                animationView.IsPlaying = false;
                progressSlider.IsEnabled = false;
            }

            //Temporary hack for https://github.com/xamarin/Xamarin.Forms/issues/6937
            await Task.Delay(1);
            playButton.IsVisible = true;
        }

        private void StartTimer()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(1), () =>
            {
                if (!timerRunning)
                    return false;

                if (animationProgress >= 1f)
                    animationProgress = 0.0f;

                animationProgress += 0.003f;

                animationView.Progress = animationProgress;
                progressSlider.Value = animationProgress;


                return timerRunning;
            });
        }

        private void progressSlider_DragCompleted(object sender, EventArgs e)
        {
            if (!(sender is Slider slider))
                return;

            playButton_Clicked(sender, e);
            animationProgress = (float)slider.Value;
        }
    }
}