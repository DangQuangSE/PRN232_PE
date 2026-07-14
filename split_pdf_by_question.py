"""
Tach PDF (dang scan anh, khong co text layer) thanh nhieu folder theo tung
cau hoi / de, moi folder chua cac anh trang lien quan (dung lam tu lieu de
tu bien soan file markdown mo ta de bai, DB diagram, UI...).

Cach dung:
    python split_pdf_by_question.py pe_trial.pdf \
        --group "Question_1:0-5" \
        --group "Question_2:5-9" \
        --outdir pe_trial \
        --dpi 200

Trong do "0-5" la chi so trang (0-based, bao gom ca 2 dau) trong file PDF goc.
Mot trang co the xuat hien trong nhieu group neu noi dung trang do la ranh
gioi giua 2 cau hoi.
"""

import argparse
import re
from pathlib import Path

import fitz


def parse_group(spec: str) -> tuple[str, list[int]]:
    name, _, pages = spec.partition(":")
    if not name or not pages:
        raise ValueError(f"Group khong hop le: '{spec}'. Dung dang Ten:start-end")
    match = re.fullmatch(r"(\d+)-(\d+)", pages)
    if not match:
        raise ValueError(f"Khoang trang khong hop le: '{pages}'. Dung dang start-end")
    start, end = int(match.group(1)), int(match.group(2))
    return name, list(range(start, end + 1))


def main() -> None:
    parser = argparse.ArgumentParser(description="Tach PDF thanh anh theo tung cau hoi/de")
    parser.add_argument("pdf", type=Path, help="Duong dan file PDF dau vao")
    parser.add_argument("--group", action="append", required=True, dest="groups", help="Ten:start-end (co the lap lai nhieu lan)")
    parser.add_argument("--outdir", type=Path, required=True, help="Thu muc goc de luu cac folder cau hoi")
    parser.add_argument("--dpi", type=int, default=200, help="Do phan giai anh xuat ra (mac dinh 200)")
    args = parser.parse_args()

    doc = fitz.open(str(args.pdf))
    args.outdir.mkdir(parents=True, exist_ok=True)

    for spec in args.groups:
        name, pages = parse_group(spec)
        group_dir = args.outdir / name / "images"
        group_dir.mkdir(parents=True, exist_ok=True)
        for page_idx in pages:
            if page_idx >= len(doc):
                print(f"[bo qua] trang {page_idx} vuot qua so trang cua PDF ({len(doc)})")
                continue
            pix = doc[page_idx].get_pixmap(dpi=args.dpi)
            out_path = group_dir / f"page_{page_idx:02d}.png"
            pix.save(str(out_path))
        print(f"{name}: da luu {len(pages)} anh vao {group_dir}")


if __name__ == "__main__":
    main()
