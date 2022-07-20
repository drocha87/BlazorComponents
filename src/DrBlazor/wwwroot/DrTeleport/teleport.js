export function teleport(source, to) {
    const target = document.querySelector(to);
    if (!target) {
        throw new Error(`teleport: ${to} is not found on the DOM`);
    }
    for (const child of source.children) {
        target.appendChild(child);
    }
    source.remove();
}
//# sourceMappingURL=teleport.js.map