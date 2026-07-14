"""
Convert a PDF (de thi / tai lieu PRN) sang file Markdown.

Giu nguyen text, bang bieu, va tach rieng cac hinh anh (UI mockup, ER diagram,
database table diagram, ...) ra thanh file anh rieng, duoc noi (link) lai vao
trong file .md bang cu phap ![alt](duong_dan_anh).

Cach dung:
    python pdf_to_md.py de_thi.pdf
    python pdf_to_md.py de_thi.pdf -o output.md --images-dir images --dpi 200

Yeu cau:
    pip install -r requirements.txt
"""

import argparse
from pathlib import Path

import pymupdf4llm


def convert_pdf_to_markdown(pdf_path: Path, output_path: Path, images_dir: Path, dpi: int) -> None:
    images_dir.mkdir(parents=True, exist_ok=True)

    md_text = pymupdf4llm.to_markdown(
        str(pdf_path),
        write_images=True,
        image_path=str(images_dir),
        image_format="png",
        dpi=dpi,
    )

    output_path.write_text(md_text, encoding="utf-8")

    image_count = sum(1 for _ in images_dir.glob("*.png"))
    print(f"Da chuyen '{pdf_path.name}' -> '{output_path}'")
    print(f"Da trich xuat {image_count} hinh anh vao thu muc '{images_dir}'")


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Chuyen PDF sang Markdown, giu lai hinh anh (UI, DB diagram, ...)")
    parser.add_argument("pdf", type=Path, help="Duong dan file PDF dau vao")
    parser.add_argument("-o", "--output", type=Path, default=None, help="Duong dan file .md dau ra (mac dinh: cung ten voi pdf)")
    parser.add_argument("--images-dir", type=Path, default=None, help="Thu muc luu hinh anh trich xuat (mac dinh: <ten_pdf>_images)")
    parser.add_argument("--dpi", type=int, default=150, help="Do phan giai anh trich xuat (mac dinh: 150)")
    return parser.parse_args()


def main() -> None:
    args = parse_args()

    pdf_path: Path = args.pdf
    if not pdf_path.exists():
        raise SystemExit(f"Khong tim thay file: {pdf_path}")

    output_path = args.output or pdf_path.with_suffix(".md")
    images_dir = args.images_dir or pdf_path.with_name(f"{pdf_path.stem}_images")

    convert_pdf_to_markdown(pdf_path, output_path, images_dir, args.dpi)


if __name__ == "__main__":
    main()
