# File Bundler CLI

A Command Line Interface (CLI) tool for bundling code files into a single file and generating response files. This tool supports various programming languages and provides options for sorting, removing empty lines, and adding metadata to the bundled file.

## Features

- **Bundle Command**: Combine code files into a single file with options to include source code origin, sort files, and remove empty lines.
- **Response File Command**: Generate response files to streamline the bundling process.

## Supported Languages

- C#
- C
- C++
- Java
- HTML
- CSS
- JavaScript
- Python
- Ruby
- Swift
- PHP

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/file-bundler-cli.git
    ```

2. Navigate to the project directory:
    ```sh
    cd file-bundler-cli
    ```

3. Build the project:
    ```sh
    dotnet build
    ```

## Usage

### Bundle Command

The `bundle` command bundles code files into a single file.

#### Options

- `-o, --output` - File path or name for the bundled output file.
- `-l, --language` - Programming languages to include in the bundle (required).
- `-n, --note` - Include source code origin as a comment (default: false).
- `-s, --sort` - Sort order for code files by letters or type (default: letter).
- `-r, --remove-empty-lines` - Remove empty lines from code files (default: false).
- `-a, --author` - Name of the author to include in the bundled file.

#### Example



```sh
dotnet run bundle --output bundled.txt --language "c# java" --note --sort type --remove-empty-lines --author "Your Name"
Development
To contribute or modify the project, follow these steps:

#### Clone the repository:


git clone https://github.com/yourusername/file-bundler-cli.git
Navigate to the project directory:

```sh
cd file-bundler-cli
```
Open the project in your preferred code editor and make your changes.

Build and test the project:
```sh
dotnet build
```
```sh
dotnet test
```
