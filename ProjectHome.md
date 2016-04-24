Chunking File System (ChunkFS) is a read-only, user-mode file system for Windows.  Each ChunkFS virtual drive is based on one or more real files, which it presents in pieces, each piece (or group of pieces) no larger than a configurable size. ChunkFS is intended to assist in archiving large files or collections of files on optical media, "spanning" and "filling" discs such as DVD-R or BD-R, when using burning programs that do not support spanning e.g. ImgBurn.

ChunkFS relies on the services of the Dokan driver,  which is a kernel-mode driver designed to enable user-mode file system drivers on Windows.

ChunkFS is licensed under the CDDL Version 1.0, an OSI-approved license.