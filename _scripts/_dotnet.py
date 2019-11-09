import subprocess
import os
import time


def build(config_file):
    if not os.path.isfile(config_file.solution_path):
        raise Exception('Solution was not found')

    command = f'dotnet publish {config_file.solution_path} ' + \
              f'--configuration {config_file.configuration} ' + \
              f'/p:AssemblyVersion="{config_file.assembly_version}" /p:Watermark="{config_file.watermark}"'

    exit_code = subprocess.call(command.split())

    return exit_code == 0


def run(dll):
    if not os.path.isfile(dll):
        raise Exception('File was not found')

    command = f'dotnet {dll}'

    process = subprocess.Popen(command.split())

    print(f'The process {process.pid} started')

    while process.poll() is None:
        time.sleep(10)  # sec

    print(f'The process {process.pid} is terminated')
