const file = document.querySelector("#file") as HTMLFieldSetElement;
const filename = document.querySelector(".filename") as HTMLSpanElement;

file.addEventListener("change", (event) => {
    const selectedFile = (event.target as HTMLInputElement).files[0];
    filename.innerHTML = selectedFile.name;
})