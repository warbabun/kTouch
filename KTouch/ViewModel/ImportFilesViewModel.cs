﻿//-----------------------------------------------------------------------
// <copyright file="ImportFilesViewModel.cs" company="Klee Group">
//     Copyright (c) Klee Group. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using KTouch.Utilities;

namespace KTouch.ViewModel {

    /// <summary>
    /// Supplies data to the MainWindow.
    /// </summary>
    public class ImportFilesViewModel {
        private const string DefaultDirectoryName = "_kTouchUnivers";
        private const string ReferenceFile = "/kTouchItems.xml";
        private ObservableCollection<string> _sourceCollection;
        private ObservableCollection<string> _resultCollection;
        private string _sourceDirectory;
        private string _destDirectory;
        private DirectoryInfo _contentDirectoryInfo;

        /// <summary>
        /// Result collection of something.
        /// </summary>
        public ObservableCollection<string> ResultCollection {
            get {
                if (_resultCollection == null) {
                    _resultCollection = new ObservableCollection<string>();
                }
                return _resultCollection;
            }
        }

        /// <summary>
        /// Source collection of filenames.
        /// </summary>
        public ObservableCollection<string> SourceCollection {
            get {
                if (_sourceCollection == null) {
                    _sourceCollection = new ObservableCollection<string>();
                }
                return _sourceCollection;
            }
        }

        /// <summary>
        /// Returns reference file directory information.
        /// </summary>
        private DirectoryInfo ContentDirectoryInfo {
            get {
                if (_contentDirectoryInfo == null) {
                    if (!string.IsNullOrEmpty(_destDirectory)) {
                        _contentDirectoryInfo = new DirectoryInfo(_destDirectory);
                    } else {
                        string subDirectory = Path.Combine(Directory.GetParent(_sourceDirectory).FullName, DefaultDirectoryName);
                        _contentDirectoryInfo = Directory.CreateDirectory(subDirectory);
                    }
                }
                return _contentDirectoryInfo;
            }
        }

        /// <summary>
        /// Directory button click handler.
        /// </summary>
        public void GetDirectory() {
            FolderBrowserDialog openDialog = new FolderBrowserDialog();
            DialogResult result = openDialog.ShowDialog();
            if (result == DialogResult.OK) {
                _sourceDirectory = openDialog.SelectedPath;
                List<string> files =
                    Directory
                    .GetFiles(_sourceDirectory, "*.*", SearchOption.AllDirectories)
                    .Where(file => SupportedExtensions.SupportedExtensionList.Contains(Path.GetExtension(file)))
                    .ToList();
                files.ForEach(file => SourceCollection.Add(file));
            }
        }

        /// <summary>
        /// Prepares all the files for processing.
        /// </summary>
        public void TransferAll() {
            FolderBrowserDialog openDialog = new FolderBrowserDialog();
            DialogResult result = openDialog.ShowDialog();
            if (result == DialogResult.OK) {
                _destDirectory = openDialog.SelectedPath;
                foreach (string file in SourceCollection) {
                    string fileCopy = CopyFile(file);
                    /* TODO: Change to asynch. */
                    ResultCollection.Add(fileCopy);
                }
                SourceCollection.Clear();
            }
        }

        /// <summary>
        /// Copies the file creating a directory to stock the related information.
        /// </summary>
        /// <param name="file">FullName to copy.</param>
        /// <returns>Destination file path.</returns>
        private string CopyFile(string file) {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
            DirectoryInfo fileDirectoryInfo = ContentDirectoryInfo.CreateSubdirectory(fileNameWithoutExtension);
            string newFilePath = Path.Combine(fileDirectoryInfo.FullName, Path.GetFileName(file));
            if (!File.Exists(newFilePath)) {
                File.Copy(file, newFilePath);
            }
            return newFilePath;
        }
    }
}