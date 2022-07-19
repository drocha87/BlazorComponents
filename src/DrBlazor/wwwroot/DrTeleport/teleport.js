export function teleport(source, to) {
    const target = document.querySelector(to);
    if (!target) {
        throw new Error("teleport: To is not found on DOM");
    }
    for (const child of source.children) {
        target.appendChild(child);
    }
    source.remove();
}
//# sourceMappingURL=teleport.js.map