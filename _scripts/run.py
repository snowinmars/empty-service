import os
import json
import click
import git
import shutil
import _dotnet
from collections import namedtuple

root = os.path.dirname(os.path.realpath(__file__))


@click.command()
@click.option('--file', '-f', required=True, help='Dll to run')
def main(file):
    print(f'Running {file}...')

    if not _dotnet.run(file):
        print(f'Cannot run {file}')


if __name__ == '__main__':
    print(f'from root {root}')
    main()

