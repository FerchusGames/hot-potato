import os
import re

def merge_cs_files(folder_path, output_file):
    cs_files = []
    for root, _, files in os.walk(folder_path):
        for file in files:
            if file.endswith(".cs"):
                cs_files.append(os.path.join(root, file))
    
    merged_content = []
    
    for file in cs_files:
        with open(file, "r", encoding="utf-8") as f:
            lines = f.readlines()
        
        for line in lines:
            # Remove lines that start with 'using' ignoring leading whitespace
            if not re.match(r'^\s*using\s+[^;]+;', line):
                merged_content.append(line)
    
    with open(output_file, "w", encoding="utf-8") as out:
        out.writelines(merged_content)
    
    print(f"Merged {len(cs_files)} files into {output_file}, removing all 'using' statements.")

folder = "./Scripts"  
output = "FileMerged.txt"
merge_cs_files(folder, output)
