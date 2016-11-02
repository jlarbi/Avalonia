using System;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="CarouselPage"/> class.
    /// </summary>
    public class CarouselPage : UserControl
    {
        private Carousel _carousel;
        private Button _left;
        private Button _right;
        private DropDown _transition;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarouselPage"/> class.
        /// </summary>
        public CarouselPage()
        {
            this.InitializeComponent();
            _left.Click += (s, e) => _carousel.Previous();
            _right.Click += (s, e) => _carousel.Next();
            _transition.SelectionChanged += TransitionChanged;
        }

        /// <summary>
        /// Initializes the carousel page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _carousel = this.FindControl<Carousel>("carousel");
            _left = this.FindControl<Button>("left");
            _right = this.FindControl<Button>("right");
            _transition = this.FindControl<DropDown>("transition");
        }

        /// <summary>
        /// Delegate called on carousel transition changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransitionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (_transition.SelectedIndex)
            {
                case 0:
                    _carousel.Transition = null;
                    break;
                case 1:
                    _carousel.Transition = new PageSlide(TimeSpan.FromSeconds(0.25));
                    break;
                case 2:
                    _carousel.Transition = new CrossFade(TimeSpan.FromSeconds(0.25));
                    break;
            }
        }
    }
}
