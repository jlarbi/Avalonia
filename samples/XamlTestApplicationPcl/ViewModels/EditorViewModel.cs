using Avalonia.Threading;

namespace XamlTestApplication.ViewModels
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Definition of the <see cref="ShellViewModel"/> class.
    /// </summary>
    public class ShellViewModel : ReactiveObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        private ShellViewModel()
        {
            documents = new ObservableCollection<EditorViewModel>();

            AddDocumentCommand = ReactiveCommand.Create();
            AddDocumentCommand.Subscribe(_ =>
            {
                Documents.Add(new EditorViewModel());
            });

            GCCommand = ReactiveCommand.Create();
            GCCommand.Subscribe(_ =>
            {
                GC.Collect();
            });
        }

        /// <summary>
        /// Gets the <see cref="ShellViewModel"/> instance.
        /// </summary>
        public static ShellViewModel Instance = new ShellViewModel();

        private ObservableCollection<EditorViewModel> documents;

        /// <summary>
        /// Gets or sets the view modle documents.
        /// </summary>
        public ObservableCollection<EditorViewModel> Documents
        {
            get { return documents; }
            set { this.RaiseAndSetIfChanged(ref documents, value); }
        }

        private EditorViewModel selectedDocument;

        /// <summary>
        /// Gets or sets the selected document.
        /// </summary>
        public EditorViewModel SelectedDocument
        {
            get { return selectedDocument; }
            set { this.RaiseAndSetIfChanged(ref selectedDocument, value); }
        }

        private int instanceCount;

        /// <summary>
        /// Gets or sets the instance count.
        /// </summary>
        public int InstanceCount
        {
            get { return instanceCount; }
            set { this.RaiseAndSetIfChanged(ref instanceCount, value); }
        }

        /// <summary>
        /// Gets a document command.
        /// </summary>
        public ReactiveCommand<object> AddDocumentCommand { get; }

        /// <summary>
        /// Gets a GC command.
        /// </summary>
        public ReactiveCommand<object> GCCommand { get; }
    }

    /// <summary>
    /// Definition of the <see cref="EditorViewModel"/> class.
    /// </summary>
    public class EditorViewModel : ReactiveObject
    {
        private static int InstanceCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorViewModel"/> class.
        /// </summary>
        public EditorViewModel()
        {
            InstanceCount++;
            ShellViewModel.Instance.InstanceCount = InstanceCount;
            text = "This is some text.";

            CloseCommand = ReactiveCommand.Create();

            CloseCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.Documents.Remove(this);
            });
        }

        /// <summary>
        /// Destroys the instance.
        /// </summary>
        ~EditorViewModel()
        {
            
            
            //System.Console.WriteLine("EVM Destructed");
            InstanceCount--;
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ShellViewModel.Instance.InstanceCount = InstanceCount;
            });

        }

        private string text;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { this.RaiseAndSetIfChanged(ref text, value); }
        }

        /// <summary>
        /// Gets a new close command.
        /// </summary>
        public ReactiveCommand<object> CloseCommand { get; }
    }
}
