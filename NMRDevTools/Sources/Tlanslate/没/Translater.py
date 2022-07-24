## Translator 参考:ExtreameRoles & SuperNewRoles
import os
import pathlib
import json
from openpyxl import load_workbook

# 通常変数
IMPORT_FILE = pathlib.Path("../NextMoreRoles/NMRDevTools/TranslateDatas.xlsx")
EXPORT_FILE = pathlib.Path("../NextMoreRoles/NextMoreRoles/Resources/TranslateDatas.csv")

# メイン処理