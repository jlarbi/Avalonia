using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Platform;

namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the abstract <see cref="FileDialog"/> class.
    /// </summary>
    public abstract class FileDialog : FileSystemDialog
    {
        /// <summary>
        /// Stores the file dialog filter(s) if any.
        /// </summary>
        public List<FileDialogFilter> Filters { get; set; } = new List<FileDialogFilter>();

        /// <summary>
        /// Gets or sets the initial file name.
        /// </summary>
        public string InitialFileName { get; set; }        
    }

    /// <summary>
    /// Definition of the abstract <see cref="FileSystemDialog"/> class.
    /// </summary>
    public abstract class FileSystemDialog : SystemDialog
    {
        /// <summary>
        /// Gets or sets the initial directory.
        /// </summary>
        public string InitialDirectory { get; set; }
    }

    /// <summary>
    /// Definition of the <see cref="SaveFileDialog"/> class.
    /// </summary>
    public class SaveFileDialog : FileDialog
    {
        /// <summary>
        /// Gets or sets the default extension.
        /// </summary>
        public string DefaultExtension { get; set; }        

        /// <summary>
        /// Shows the save file dialog
        /// </summary>
        /// <param name="window">The parent window if any.</param>
        /// <returns></returns>
        public async Task<string> ShowAsync(Window window = null)
            =>
                ((await AvaloniaLocator.Current.GetService<ISystemDialogImpl>().ShowFileDialogAsync(this, window?.PlatformImpl)) ??
                 new string[0]).FirstOrDefault();
    }

    /// <summary>
    /// Definition of the <see cref="OpenFileDialog"/> class.
    /// </summary>
    public class OpenFileDialog : FileDialog
    {
        /// <summary>
        /// Gets or sets the flag indicating whether multiple files can be opened at once or not.
        /// </summary>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// Shows the open file dialog
        /// </summary>
        /// <param name="window">The parent window if any.</param>
        /// <returns></returns>
        public Task<string[]> ShowAsync(Window window = null)
            => AvaloniaLocator.Current.GetService<ISystemDialogImpl>().ShowFileDialogAsync(this, window?.PlatformImpl);
    }

    /// <summary>
    /// Definition of the <see cref="OpenFolderDialog"/> class.
    /// </summary>
    public class OpenFolderDialog : FileSystemDialog
    {
        /// <summary>
        /// Gets or sets the default directory.
        /// </summary>
        public string DefaultDirectory { get; set; }

        /// <summary>
        /// Shows the open folder dialog
        /// </summary>
        /// <param name="window">The parent window if any.</param>
        /// <returns></returns>
        public Task<string> ShowAsync(Window window = null)
               => AvaloniaLocator.Current.GetService<ISystemDialogImpl>().ShowFolderDialogAsync(this, window?.PlatformImpl);
    }

    /// <summary>
    /// Definition of abstract the <see cref="SystemDialog"/> class.
    /// </summary>
    public abstract class SystemDialog
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// Definition of the <see cref="FileDialogFilter"/> class.
    /// </summary>
    public class FileDialogFilter
    {
        /// <summary>
        /// Gets or sets the filter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the extension(s) to filter.
        /// </summary>
        public List<string> Extensions { get; set; } = new List<string>();
    }
}
