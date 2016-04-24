# Introduction #

ChunkFS has minimal doc, just a readme so far. Here is a copy of it.


# Details #

1. Introduction

ChunkFS (Chunking File System) is a specialized read-only user-mode file system for windows.

Use of ChunkFS is only permitted under the terms of the accompanying licenses.

The present version of ChunkFS is "beta" software. This means, not only is the user required by the license to accept complete responsibility for all consequences of using the software, the user is advised the present version has not been widely or thoroughly tested under varying conditions and that there is a significant chance that it may not function as intended or expected on any given configuration of hardware and software, including the potential to cause system-level malfunction or "Blue Screen" failure.

ChunkFS relies on the Dokan library and driver, version 0.60. For more information about Dokan, and to download the required Dokan driver installer, see http://dokan-dev.net/en/

ChunkFS requires the Microsoft .NET library version 2 to run. If you need to install .NET Version 2, see http://www.microsoft.com/download/en/details.aspx?id=19 or other suitable Microsoft pages e.g. for service-pack releases.

A ChunkFS virtual drive is configured by selecting some source file(s) and indicating a target chunk size. When the given configuration is mounted, it appears to Windows as a file system containing a number of read-only files, where each file (or file group) is limited to the target size. The "chunking" is accomplished entirely by internal calculations and by returning fabricated file names and properties to standard windows filesystem functions; ChunkFS makes no changes of any kind to the input files. Note: results are undefined and unknown if any other process attempts to write to a file while it is being accessed via a ChunkFS virtual drive.

ChunkFS does not use and is not built with the Microsoft Driver Development Kit. Instead, ChunkFS is written in c# and can be edited and compiled using Microsoft Visual Studio 2008 or later. Thus ChunkFS itself runs entirely in "user mode" and does not contain any code that runs in privileged or "kernel mode". Instead, ChunkFS builds on the capabilities of Dokan, which provides a kernel-mode driver with the purpose and function of allowing Windows file systems to be written in user-mode code.

Throughput rates have been observed to vary widely in testing on different configurations. At one end, running in a virtual machine with the input files on a mounted network volume and using Windows Explorer to copy chunks out of the ChunkFS virtual drive to the same pysical drive over the network, sustained rates of under 9MB/sec were recorded. At the other end, a similar test but with the OS installed natively and using two different physical drives for the source and target, sustained rates well in excess of 60MB/sec were recorded.

Good burns to DVD-R and BD-R have been made with commonly used buring software directly reading "chunks" from a mounted ChunkFS virtual drive. Bit-for-bit comparison of files restored from multiple BD-R have shown the restored file to be identical to the original. As with any software especially Beta software it is possible that your results may differ from these.

2. Purpose

ChunkFS was developed primarily to attempt to meet two closely related requirements for users wishing to archive large files or groups of files to optical media using simple burning utilities:

1. (virtually) break large files into smaller pieces as needed to fit on the available optical media

2. fill each piece of optical media rather than leaving large amounts of unused, wasted space

ChunkFS was developed secondarily to attempt to meet two additional requirements concerning the use of the archived data

3. restore directly from the archival media, with awareness of the correct number and sequence of chunks

4. for certain special cases, be able to use the archived chunks directly without restoring

The special cases are presently limited to mpeg files and transport stream files. If the option is selected, ChunkFS will search (for a limited distance) backwards from the nominal cut point, looking for a suitable location that is likely to result in a post-cut file that is directly playable by many media players.

3. Installation

If not already installed, download and install the Microsoft .NET Framework Version 2.

If not already installed, download and install Dokan driver version 0.6.

If a previous version of ChunkFS is installed, uninstall it using the option in the Windows Start menu. Note: this will not delete any configuration you may have previously saved; if a saved configuration is still compatible with the new version it will be loaded on start.

Run the ChunkFS installer, setupChunkFS.exe. Follow the prompts to install ChunkFS. You must accept the license terms, displayed by the setup program, in order to install and use ChunkFS.

4. Configuration and Use

The ChunkFS gui presents two grids and some action buttons. In the left-hand grid you can create and edit one or more ChunkFS drive configurations. In the right hand grid you can enter and edit the names of the file(s) that are to be sources of the content in the currently selected configuration on the left.

Quotes where used below are to set off the value in the text and are NOT to be included in the actual value.

Drive Configuration properties

Name. A name for the configuration. This is used in saving the configuration and also to form part of the volume name when mounted. Does not need to be unique. Must contain only characters that are allowed in a Windows volume name.

Mount Point. Windows drive designator, e.g. "M:\". Cannot be "A:\" or "B:\". Mount will fail if Windows already has any volume at this drive letter. Need not be unique, but only one configuration can be mouted at a given drive letter at one time.

Chunk Size. Target size for the apparent files and/or file groups. This can be expressed as an absolute number or a number of K, M, or G. In ChunkFS a K is 1024, not 1000, and the larger sizes accordingly. Can include decimals. Example: 22.5G is a conservative fill (leaves a small outer margin) for a single layer BR disc. Example: 4400M is a conservative fill (leaves a small outer margin) for a single layer DVD-R disc.

Use Ext. The usual chunk name is formed by extending the base file name with a chunkid string of the form .filedate.chunknumber.chunksinfile which is especially useful if some shell extension locks or otherwise manipulates files with the original extention. Select this option to have the chunks carry the extention of the original file, in which case the chunkid information precedes it.

Fill. Select this option to have the first chunk of files after the first one made smaller than than full size, as and if needed to fill leftover free space from the previous chunk(s).

Subdirs. Only meaningful in combination with Fill:true. Select this option to have the virtual file system also present subdirectories numbered from 001 with each containing the sequential file pieces needed to fill up the chunksize. This makes it easier to understand what Fill:true does and to work with the results.

Smart. Select this to have ChunkFS attempt to align the chunks of mpg and transport stream files at meaningful packet boundaries in the hope that the archived chunks will be separately playable. Only useful in combination with Use Ext:true. When selected some chunks may be slightly smaller than the target size.

Minimum. Enter a value to avoid the possibility of making very small part-1 chunks when using Fill. This is a decimal number representing the portion of "free space" to leave unfilled. Example: with Chunk Size:4400M and Minimum:0.01 then up to 44M of leftover free space will be ignored, thus no part-1 chunk of less than 44M will be made. Zero disables. At least a very small non-zero value is strongly recommended if using Fill:true.

Mounted. Read-only indicator showing if the configuration is presently mounted and visible e.g. in the Windows Explorer.

Fileset properties

File Name. The full path or file name pattern for source file(s). ChunkFS never modifies the actual input files. Pattern can include the wildcards "`*`" or "?".

Total size. Read-only, shows the size of the file(s) selected by the file name (pattern).

Drive Configuation Actions

Mount. Mount the selected drive configuration so it is visible to Windows and applications. Button will not enable unless the selected configuration has enough information to run.

Unmount. Unmount the selected drive configuration. Button will only enable if the selected configuration is mounted.

Monitor. Open a popup window to display running statistics for the drive. Button will only enable if the selected configuration is mounted.

Save Config. Save the current configuration to an XML file under the windows application data directory. You are also prompted to optionally save when closing ChunkFS gui.

Load Config. Clear the current configuration and reload from the saved XML file.

Fileset Actions

Add. Opens a dialog allowing selection of one or more files to add to the currently selected drive configuration.

Up. Move the selected file entry up in the list.

Down. Move the selected file entry up in the list.

Miscelaneous Actions

Restore. Opens a popup window allowing you to restore a chunked file.

About. Displays the About box.

5. Command Line Utilities.

The Start menu includes an item "ChunkFS Command Line" that should open a command window at the directory where the ChunkFS programs are installed. The two utilities are

CFSRestore.exe - restore a chunked file.

Usage: `CFSRestore <firstfile> <destdir>`

ChunkFS.exe - mount a ChunkFS file system.

Usage: `ChunkFS <mountpoint> <chunksize> <sourcefile>... `

These utilities must be run with the directory where the ChunkFS programs are installed as the current working directory.