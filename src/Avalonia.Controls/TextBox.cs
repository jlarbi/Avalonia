// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Input.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Controls.Utils;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Data;

namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the <see cref="TextBox"/> class.
    /// </summary>
    public class TextBox : TemplatedControl, UndoRedoHelper<TextBox.UndoRedoState>.IUndoRedoHost
    {
        /// <summary>
        /// The accepts return property
        /// </summary>
        public static readonly StyledProperty<bool> AcceptsReturnProperty =
            AvaloniaProperty.Register<TextBox, bool>("AcceptsReturn");

        /// <summary>
        /// The accepts tab property.
        /// </summary>
        public static readonly StyledProperty<bool> AcceptsTabProperty =
            AvaloniaProperty.Register<TextBox, bool>("AcceptsTab");

        /// <summary>
        /// The CanScrollHorizontally property.
        /// </summary>
        public static readonly DirectProperty<TextBox, bool> CanScrollHorizontallyProperty =
            AvaloniaProperty.RegisterDirect<TextBox, bool>("CanScrollHorizontally", o => o.CanScrollHorizontally);

        /// <summary>
        /// The CaretIndex property.
        /// </summary>
        public static readonly DirectProperty<TextBox, int> CaretIndexProperty =
            AvaloniaProperty.RegisterDirect<TextBox, int>(
                nameof(CaretIndex),
                o => o.CaretIndex,
                (o, v) => o.CaretIndex = v);

        /// <summary>
        /// The DataValidationErrors property.
        /// </summary>
        public static readonly DirectProperty<TextBox, IEnumerable<Exception>> DataValidationErrorsProperty =
            AvaloniaProperty.RegisterDirect<TextBox, IEnumerable<Exception>>(
                nameof(DataValidationErrors),
                o => o.DataValidationErrors);

        /// <summary>
        /// The Is ReadOnly property.
        /// </summary>
        public static readonly StyledProperty<bool> IsReadOnlyProperty =
            AvaloniaProperty.Register<TextBox, bool>(nameof(IsReadOnly));

        /// <summary>
        /// The selection start property.
        /// </summary>
        public static readonly DirectProperty<TextBox, int> SelectionStartProperty =
            AvaloniaProperty.RegisterDirect<TextBox, int>(
                nameof(SelectionStart),
                o => o.SelectionStart,
                (o, v) => o.SelectionStart = v);

        /// <summary>
        /// The selection end property.
        /// </summary>
        public static readonly DirectProperty<TextBox, int> SelectionEndProperty =
            AvaloniaProperty.RegisterDirect<TextBox, int>(
                nameof(SelectionEnd),
                o => o.SelectionEnd,
                (o, v) => o.SelectionEnd = v);

        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly DirectProperty<TextBox, string> TextProperty =
            TextBlock.TextProperty.AddOwner<TextBox>(
                o => o.Text,
                (o, v) => o.Text = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true);

        /// <summary>
        /// The text alignment property.
        /// </summary>
        public static readonly StyledProperty<TextAlignment> TextAlignmentProperty =
            TextBlock.TextAlignmentProperty.AddOwner<TextBox>();

        /// <summary>
        /// The text wrapping property.
        /// </summary>
        public static readonly StyledProperty<TextWrapping> TextWrappingProperty =
            TextBlock.TextWrappingProperty.AddOwner<TextBox>();

        /// <summary>
        /// The watermark property.
        /// </summary>
        public static readonly StyledProperty<string> WatermarkProperty =
            AvaloniaProperty.Register<TextBox, string>("Watermark");

        /// <summary>
        /// The UseFloatingWatermark property.
        /// </summary>
        public static readonly StyledProperty<bool> UseFloatingWatermarkProperty =
            AvaloniaProperty.Register<TextBox, bool>("UseFloatingWatermark");

        /// <summary>
        /// Definition of the <see cref="UndoRedoState"/> structure.
        /// </summary>
        struct UndoRedoState : IEquatable<UndoRedoState>
        {
            /// <summary>
            /// Gets the text.
            /// </summary>
            public string Text { get; }

            /// <summary>
            /// Gets the caret position.
            /// </summary>
            public int CaretPosition { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="UndoRedoState"/> class.
            /// </summary>
            /// <param name="text"></param>
            /// <param name="caretPosition"></param>
            public UndoRedoState(string text, int caretPosition)
            {
                Text = text;
                CaretPosition = caretPosition;
            }

            /// <summary>
            /// Checks whether this instance is equal to another.
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(UndoRedoState other) => ReferenceEquals(Text, other.Text) || Equals(Text, other.Text);
        }

        private string _text;
        private int _caretIndex;
        private int _selectionStart;
        private int _selectionEnd;
        private bool _canScrollHorizontally;
        private TextPresenter _presenter;
        private UndoRedoHelper<UndoRedoState> _undoRedoHelper;
        private bool _ignoreTextChanges;
        private IEnumerable<Exception> _dataValidationErrors;

        /// <summary>
        /// Initializes static member(s) of the <see cref="TextBlock"/> class.
        /// </summary>
        static TextBox()
        {
            FocusableProperty.OverrideDefaultValue(typeof(TextBox), true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBlock"/> class.
        /// </summary>
        public TextBox()
        {
            this.GetObservable(TextWrappingProperty)
                .Select(x => x == TextWrapping.NoWrap)
                .Subscribe(x => CanScrollHorizontally = x);

            var horizontalScrollBarVisibility = this.GetObservable(AcceptsReturnProperty)
                .Select(x => x ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden);

            Bind(
                ScrollViewer.HorizontalScrollBarVisibilityProperty,
                horizontalScrollBarVisibility,
                BindingPriority.Style);
            _undoRedoHelper = new UndoRedoHelper<UndoRedoState>(this);
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the text box accepts return key or not.
        /// </summary>
        public bool AcceptsReturn
        {
            get { return GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the text box accepts Tab key or not.
        /// </summary>
        public bool AcceptsTab
        {
            get { return GetValue(AcceptsTabProperty); }
            set { SetValue(AcceptsTabProperty, value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the text box can scroll horizontally or not.
        /// </summary>
        public bool CanScrollHorizontally
        {
            get { return _canScrollHorizontally; }
            private set { SetAndRaise(CanScrollHorizontallyProperty, ref _canScrollHorizontally, value); }
        }

        /// <summary>
        /// Gets or sets the caret index.
        /// </summary>
        public int CaretIndex
        {
            get
            {
                return _caretIndex;
            }

            set
            {
                value = CoerceCaretIndex(value);
                SetAndRaise(CaretIndexProperty, ref _caretIndex, value);
                UndoRedoState state;
                if (_undoRedoHelper.TryGetLastState(out state) && state.Text == Text)
                    _undoRedoHelper.UpdateLastState();
            }
        }

        /// <summary>
        /// Gets the data validation errors if any.
        /// </summary>
        public IEnumerable<Exception> DataValidationErrors
        {
            get { return _dataValidationErrors; }
            private set { SetAndRaise(DataValidationErrorsProperty, ref _dataValidationErrors, value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the text box is read only or not.
        /// </summary>
        public bool IsReadOnly
        {
            get { return GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection start index.
        /// </summary>
        public int SelectionStart
        {
            get
            {
                return _selectionStart;
            }

            set
            {
                value = CoerceCaretIndex(value);
                SetAndRaise(SelectionStartProperty, ref _selectionStart, value);
            }
        }

        /// <summary>
        /// Gets or sets the selection end index.
        /// </summary>
        public int SelectionEnd
        {
            get
            {
                return _selectionEnd;
            }

            set
            {
                value = CoerceCaretIndex(value);
                SetAndRaise(SelectionEndProperty, ref _selectionEnd, value);
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [Content]
        public string Text
        {
            get { return _text; }
            set
            {
                if (!_ignoreTextChanges)
                {
                    SetAndRaise(TextProperty, ref _text, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the watermask.
        /// </summary>
        public string Watermark
        {
            get { return GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the text box uses floating watermask or not.
        /// </summary>
        public bool UseFloatingWatermark
        {
            get { return GetValue(UseFloatingWatermarkProperty); }
            set { SetValue(UseFloatingWatermarkProperty, value); }
        }

        /// <summary>
        /// Gets or sets the wrapping mode of the text.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get { return GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// Delegate called on template applied.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            _presenter = e.NameScope.Get<TextPresenter>("PART_TextPresenter");
            _presenter.Cursor = new Cursor(StandardCursorType.Ibeam);
        }

        /// <summary>
        /// Delegate called on text box got focus
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);

            // when navigating to a textbox via the tab key, select all text if
            //   1) this textbox is *not* a multiline textbox
            //   2) this textbox has any text to select
            if (e.NavigationMethod == NavigationMethod.Tab &&
                !AcceptsReturn &&
                Text?.Length > 0)
            {
                SelectionStart = 0;
                SelectionEnd = Text.Length;
            }
            else
            {
                _presenter.ShowCaret();
            }
        }

        /// <summary>
        /// Delegate called on text box lost focus.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            SelectionStart = 0;
            SelectionEnd = 0;
            _presenter.HideCaret();
        }

        /// <summary>
        /// Delegate called on new text input.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextInput(TextInputEventArgs e)
        {
            HandleTextInput(e.Text);
        }

        /// <summary>
        /// Handles a new text input.
        /// </summary>
        /// <param name="input">The new text input.</param>
        private void HandleTextInput(string input)
        {
            if (!IsReadOnly)
            {
                string text = Text ?? string.Empty;
                int caretIndex = CaretIndex;
                if (!string.IsNullOrEmpty(input))
                {
                    DeleteSelection();
                    caretIndex = CaretIndex;
                    text = Text ?? string.Empty;
                    SetTextInternal(text.Substring(0, caretIndex) + input + text.Substring(caretIndex));
                    CaretIndex += input.Length;
                    SelectionStart = SelectionEnd = CaretIndex;
                    _undoRedoHelper.DiscardRedo();
                }
            }
        }

        /// <summary>
        /// Copies the text being in the box.
        /// </summary>
        private async void Copy()
        {
            await ((IClipboard)AvaloniaLocator.Current.GetService(typeof(IClipboard)))
                .SetTextAsync(GetSelection());
        }

        /// <summary>
        /// Pastes the text copied in the text box.
        /// </summary>
        private async void Paste()
        {
            var text = await ((IClipboard)AvaloniaLocator.Current.GetService(typeof(IClipboard))).GetTextAsync();
            if (text == null)
            {
                return;
            }
            _undoRedoHelper.Snapshot();
            HandleTextInput(text);
        }

        /// <summary>
        /// Delegate called on key down, the text box being focused.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            string text = Text ?? string.Empty;
            int caretIndex = CaretIndex;
            bool movement = false;
            bool handled = true;
            var modifiers = e.Modifiers;

            switch (e.Key)
            {
                case Key.A:
                    if (modifiers == InputModifiers.Control)
                    {
                        SelectAll();
                    }

                    break;
                case Key.C:
                    if (modifiers == InputModifiers.Control)
                    {
                        Copy();
                    }
                    break;

                case Key.X:
                    if (modifiers == InputModifiers.Control)
                    {
                        Copy();
                        DeleteSelection();
                    }
                    break;

                case Key.V:
                    if (modifiers == InputModifiers.Control)
                    {
                        Paste();
                    }

                    break;

                case Key.Z:
                    if (modifiers == InputModifiers.Control)
                        _undoRedoHelper.Undo();

                    break;
                case Key.Y:
                    if (modifiers == InputModifiers.Control)
                        _undoRedoHelper.Redo();

                    break;
                case Key.Left:
                    MoveHorizontal(-1, modifiers);
                    movement = true;
                    break;

                case Key.Right:
                    MoveHorizontal(1, modifiers);
                    movement = true;
                    break;

                case Key.Up:
                    MoveVertical(-1, modifiers);
                    movement = true;
                    break;

                case Key.Down:
                    MoveVertical(1, modifiers);
                    movement = true;
                    break;

                case Key.Home:
                    MoveHome(modifiers);
                    movement = true;
                    break;

                case Key.End:
                    MoveEnd(modifiers);
                    movement = true;
                    break;

                case Key.Back:
                    if (modifiers == InputModifiers.Control && SelectionStart == SelectionEnd)
                    {
                        SetSelectionForControlBackspace(modifiers);
                    }

                    if (!DeleteSelection() && CaretIndex > 0)
                    {
                        var removedCharacters = 1;
                        // handle deleting /r/n
                        // you don't ever want to leave a dangling /r around. So, if deleting /n, check to see if 
                        // a /r should also be deleted.
                        if (CaretIndex > 1 &&
                            text[CaretIndex - 1] == '\n' &&
                            text[CaretIndex - 2] == '\r')
                        {
                            removedCharacters = 2;
                        }

                        SetTextInternal(text.Substring(0, caretIndex - removedCharacters) + text.Substring(caretIndex));
                        CaretIndex -= removedCharacters;
                        SelectionStart = SelectionEnd = CaretIndex;
                    }

                    break;

                case Key.Delete:
                    if (modifiers == InputModifiers.Control && SelectionStart == SelectionEnd)
                    {
                        SetSelectionForControlDelete(modifiers);
                    }

                    if (!DeleteSelection() && caretIndex < text.Length)
                    {
                        var removedCharacters = 1;
                        // handle deleting /r/n
                        // you don't ever want to leave a dangling /r around. So, if deleting /n, check to see if 
                        // a /r should also be deleted.
                        if (CaretIndex < text.Length - 1 &&
                            text[caretIndex + 1] == '\n' &&
                            text[caretIndex] == '\r')
                        {
                            removedCharacters = 2;
                        }

                        SetTextInternal(text.Substring(0, caretIndex) + text.Substring(caretIndex + removedCharacters));
                    }

                    break;

                case Key.Enter:
                    if (AcceptsReturn)
                    {
                        HandleTextInput("\r\n");
                    }

                    break;

                case Key.Tab:
                    if (AcceptsTab)
                    {
                        HandleTextInput("\t");
                    }
                    else
                    {
                        base.OnKeyDown(e);
                        handled = false;
                    }

                    break;

                default:
                    handled = false;
                    break;
            }

            if (movement && ((modifiers & InputModifiers.Shift) != 0))
            {
                SelectionEnd = CaretIndex;
            }
            else if (movement)
            {
                SelectionStart = SelectionEnd = CaretIndex;
            }

            if (handled)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Delegate called on pointer pressed on the text box.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (e.Source == _presenter)
            {
                var point = e.GetPosition(_presenter);
                var index = CaretIndex = _presenter.GetCaretIndex(point);
                var text = Text;

                if (text != null)
                {
                    switch (e.ClickCount)
                    {
                        case 1:
                            SelectionStart = SelectionEnd = index;
                            break;
                        case 2:
                            if (!StringUtils.IsStartOfWord(text, index))
                            {
                                SelectionStart = StringUtils.PreviousWord(text, index);
                            }

                            SelectionEnd = StringUtils.NextWord(text, index);
                            break;
                        case 3:
                            SelectionStart = 0;
                            SelectionEnd = text.Length;
                            break;
                    }
                }

                e.Device.Capture(_presenter);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Delegate called on pointer moved on the text box.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (_presenter != null && e.Device.Captured == _presenter)
            {
                var point = e.GetPosition(_presenter);
                CaretIndex = SelectionEnd = _presenter.GetCaretIndex(point);
            }
        }

        /// <summary>
        /// Delegate called on pointer released from the text box.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPointerReleased(PointerEventArgs e)
        {
            if (_presenter != null && e.Device.Captured == _presenter)
            {
                e.Device.Capture(null);
            }
        }

        /// <summary>
        /// Updates the data validation errors.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="status"></param>
        protected override void UpdateDataValidation(AvaloniaProperty property, BindingNotification status)
        {
            if (property == TextProperty)
            {
                var classes = (IPseudoClasses)Classes;
                DataValidationErrors = UnpackException(status.Error);
                classes.Set(":error", DataValidationErrors != null);
            }
        }

        /// <summary>
        /// Unpacks the given exception.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static IEnumerable<Exception> UnpackException(Exception exception)
        {
            if (exception != null)
            {
                var aggregate = exception as AggregateException;
                var exceptions = aggregate == null ?
                    (IEnumerable<Exception>)new[] { exception } :
                    aggregate.InnerExceptions;
                var filtered = exceptions.Where(x => !(x is BindingChainException)).ToList();

                if (filtered.Count > 0)
                {
                    return filtered;
                }
            }

            return null;
        }

        /// <summary>
        /// Coerces the caret index on changes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int CoerceCaretIndex(int value)
        {
            var text = Text;
            var length = text?.Length ?? 0;

            if (value < 0)
            {
                return 0;
            }
            else if (value > length)
            {
                return length;
            }
            else if (value > 0 && text[value - 1] == '\r' && text[value] == '\n')
            {
                return value + 1;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Deletes the character at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int DeleteCharacter(int index)
        {
            var start = index + 1;
            var text = Text;
            var c = text[index];
            var result = 1;

            if (c == '\n' && index > 0 && text[index - 1] == '\r')
            {
                --index;
                ++result;
            }
            else if (c == '\r' && index < text.Length - 1 && text[index + 1] == '\n')
            {
                ++start;
                ++result;
            }

            Text = text.Substring(0, index) + text.Substring(start);

            return result;
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="modifiers"></param>
        private void MoveHorizontal(int direction, InputModifiers modifiers)
        {
            var text = Text ?? string.Empty;
            var caretIndex = CaretIndex;

            if ((modifiers & InputModifiers.Control) == 0)
            {
                var index = caretIndex + direction;

                if (index < 0 || index > text.Length)
                {
                    return;
                }
                else if (index == text.Length)
                {
                    CaretIndex = index;
                    return;
                }

                var c = text[index];

                if (direction > 0)
                {
                    CaretIndex += (c == '\r' && index < text.Length - 1 && text[index + 1] == '\n') ? 2 : 1;
                }
                else
                {
                    CaretIndex -= (c == '\n' && index > 0 && text[index - 1] == '\r') ? 2 : 1;
                }
            }
            else
            {
                if (direction > 0)
                {
                    CaretIndex += StringUtils.NextWord(text, caretIndex) - caretIndex;
                }
                else
                {
                    CaretIndex += StringUtils.PreviousWord(text, caretIndex) - caretIndex;
                }
            }
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="count"></param>
        /// <param name="modifiers"></param>
        private void MoveVertical(int count, InputModifiers modifiers)
        {
            var formattedText = _presenter.FormattedText;
            var lines = formattedText.GetLines().ToList();
            var caretIndex = CaretIndex;
            var lineIndex = GetLine(caretIndex, lines) + count;

            if (lineIndex >= 0 && lineIndex < lines.Count)
            {
                var line = lines[lineIndex];
                var rect = formattedText.HitTestTextPosition(caretIndex);
                var y = count < 0 ? rect.Y : rect.Bottom;
                var point = new Point(rect.X, y + (count * (line.Height / 2)));
                var hit = formattedText.HitTestPoint(point);
                CaretIndex = hit.TextPosition + (hit.IsTrailing ? 1 : 0);
            }
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="modifiers"></param>
        private void MoveHome(InputModifiers modifiers)
        {
            var text = Text ?? string.Empty;
            var caretIndex = CaretIndex;

            if ((modifiers & InputModifiers.Control) != 0)
            {
                caretIndex = 0;
            }
            else
            {
                var lines = _presenter.FormattedText.GetLines();
                var pos = 0;

                foreach (var line in lines)
                {
                    if (pos + line.Length > caretIndex || pos + line.Length == text.Length)
                    {
                        break;
                    }

                    pos += line.Length;
                }

                caretIndex = pos;
            }

            CaretIndex = caretIndex;
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="modifiers"></param>
        private void MoveEnd(InputModifiers modifiers)
        {
            var text = Text ?? string.Empty;
            var caretIndex = CaretIndex;

            if ((modifiers & InputModifiers.Control) != 0)
            {
                caretIndex = text.Length;
            }
            else
            {
                var lines = _presenter.FormattedText.GetLines();
                var pos = 0;

                foreach (var line in lines)
                {
                    pos += line.Length;

                    if (pos > caretIndex)
                    {
                        if (pos < text.Length)
                        {
                            --pos;
                            if (pos > 0 && Text[pos - 1] == '\r' && Text[pos] == '\n')
                            {
                                --pos;
                            }
                        }

                        break;
                    }
                }

                caretIndex = pos;
            }

            CaretIndex = caretIndex;
        }

        /// <summary>
        /// Selects all the text box content.
        /// </summary>
        private void SelectAll()
        {
            SelectionStart = 0;
            SelectionEnd = Text?.Length ?? 0;
        }

        /// <summary>
        /// Deletes the selection.
        /// </summary>
        /// <returns>True if deleted, false otherwise.</returns>
        private bool DeleteSelection()
        {
            if (!IsReadOnly)
            {
                var selectionStart = SelectionStart;
                var selectionEnd = SelectionEnd;

                if (selectionStart != selectionEnd)
                {
                    var start = Math.Min(selectionStart, selectionEnd);
                    var end = Math.Max(selectionStart, selectionEnd);
                    var text = Text;
                    SetTextInternal(text.Substring(0, start) + text.Substring(end));
                    SelectionStart = SelectionEnd = CaretIndex = start;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the current selection.
        /// </summary>
        /// <returns></returns>
        private string GetSelection()
        {
            var selectionStart = SelectionStart;
            var selectionEnd = SelectionEnd;
            var start = Math.Min(selectionStart, selectionEnd);
            var end = Math.Max(selectionStart, selectionEnd);
            if (start == end || (Text?.Length ?? 0) < end)
            {
                return "";
            }
            return Text.Substring(start, end - start);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="caretIndex"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        private int GetLine(int caretIndex, IList<FormattedTextLine> lines)
        {
            int pos = 0;
            int i;

            for (i = 0; i < lines.Count; ++i)
            {
                var line = lines[i];
                pos += line.Length;

                if (pos > caretIndex)
                {
                    break;
                }
            }

            return i;
        }

        /// <summary>
        /// Sets the given text value into the tex box.
        /// </summary>
        /// <param name="value"></param>
        private void SetTextInternal(string value)
        {
            try
            {
                _ignoreTextChanges = true;
                SetAndRaise(TextProperty, ref _text, value);
            }
            finally
            {
                _ignoreTextChanges = false;
            }
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="modifiers"></param>
        private void SetSelectionForControlBackspace(InputModifiers modifiers)
        {
            SelectionStart = CaretIndex;
            MoveHorizontal(-1, modifiers);
            SelectionEnd = CaretIndex;
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="modifiers"></param>
        private void SetSelectionForControlDelete(InputModifiers modifiers)
        {
            SelectionStart = CaretIndex;
            MoveHorizontal(1, modifiers);
            SelectionEnd = CaretIndex;
        }

        /// <summary>
        /// Gets the undo redo state.
        /// </summary>
        UndoRedoState UndoRedoHelper<UndoRedoState>.IUndoRedoHost.UndoRedoState
        {
            get { return new UndoRedoState(Text, CaretIndex); }
            set
            {
                Text = value.Text;
                SelectionStart = SelectionEnd = CaretIndex = value.CaretPosition;
            }
        }
    }
}
