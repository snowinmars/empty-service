import os
import json
import shutil

import click  # click
import git  # gitpython

import _dotnet

root = os.path.dirname(os.path.realpath(__file__))


class JSONObject:
    def __init__(self, dictionary):
        vars(self).update(dictionary)


def json2obj(data):
    return json.loads(data, object_hook=JSONObject)


def join_path(path, *pathes):
    return os.path.abspath(os.path.join(path, *pathes))


def patch_config_pathes(config):
    #  config contains solution-related pathes, but the absolute pathes are required
    related = '..'

    config.output = join_path(root, related, config.output)
    config.solution_path = join_path(root, related, config.solution_path)

    resources = []
    for resource in config.resources:
        resources.append(join_path(root, related, resource))
    config.resources = resources

    return config


@click.command()
@click.option('--config_file', '-f', required=True, help='File that contains build configuration')
@click.option('--watermark', '-h', required=False, help='Watermark that will be hardcoded to dlls.' +
                                                        ' If not provided - the script will try to get current' +
                                                        ' repository commit hash and throw an error on failure')
def main(config_file, watermark):
    config = patch_config_pathes(json2obj(open(config_file, 'r').read()))

    if watermark:
        config.watermark = watermark
    else:
        repo = git.Repo(search_parent_directories=True)
        sha = repo.head.object.hexsha
        config.watermark = sha

    print(f"Watermark is {config.watermark}")

    if os.path.isdir(config.output):
        print(f"Cleaning output folder {config.output}")
        shutil.rmtree(config.output)
    else:
        os.mkdir(config.output)

    print("Copying resources")
    i = 1
    for filepath in config.resources:
        filename = os.path.basename(filepath)
        destination = join_path(config.output, filename)

        print(f"    [{i} / {len(config.resources)}]: '{filepath}' > '{destination}'")

        if os.path.isdir(filepath):
            shutil.copytree(filepath, destination)
        elif os.path.isfile(filepath):
            shutil.copyfile(filepath, destination)
        else:
            raise Exception(f'File {filepath} was not found')

        i += 1

    print(f"Building from {config.watermark} commit in {config.configuration} configuration using {config_file}")

    if not _dotnet.build(config):
        print('Build failed')


if __name__ == '__main__':
    print(f'from root {root}')
    main()
