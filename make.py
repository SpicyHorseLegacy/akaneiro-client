#!/usr/bin/env python

from os import path
from logging import getLogger, StreamHandler, DEBUG
from argparse import ArgumentParser
from subprocess import check_call, STDOUT

UNITY_BIN = "/Applications/Unity_4.2.2/Unity.app/Contents/MacOS/Unity"

class Main(object):
	def __init__(self):
		super(Main, self).__init__()
		self.logger = getLogger("main")
		self.logger.setLevel(DEBUG)
		self.logger.addHandler(StreamHandler())

	def __call__(self):
		parser = ArgumentParser(description='Unity3D build')
		parser.add_argument('targets', nargs="+", type=unicode, choices=['BuildBundleAll', 'BuildBundleScenes'])
		args = parser.parse_args()

		for target in args.targets:
			command = [ 
				UNITY_BIN,
				"-quit", "-batchmode", "-nographics",
				"-projectPath", path.dirname(path.abspath(__file__)),
				"-executeMethod", target + ".Execute"
			]
			check_call(command, stderr=STDOUT)

if __name__ == '__main__':
	Main()()
