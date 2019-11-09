import os
import shutil

root = os.path.dirname(os.path.realpath(__file__))


def join_path(path, *pathes):
    return os.path.abspath(os.path.join(path, *pathes))


def purge(directory, does_match):
    for base_root, child_directories, child_files in os.walk(directory):
        for child_directory in child_directories:
            if does_match(child_directory):
                child = join_path(base_root, child_directory)
                print(f'Removing {child}')
                try:
                    shutil.rmtree(child)
                except PermissionError:
                    print(f'Cannot remove {child} due to access error')


def main():
    output_folders = ['bin', 'obj', '_output', 'packages']
    sln_path = join_path(root, '..')

    purge(sln_path, lambda x: x in output_folders)


if __name__ == '__main__':
    print(f'from root {root}')
    main()
