export function teleport(source, to, wrapper) {
    const target = document.querySelector(to);
    if (!target) {
        throw new Error(`teleport: ${to} is not found on the DOM`);
    }
    target.appendChild(source);
    console.log(wrapper);
}
export function removeFromDOM(el) {
    if (el && el.__internalId !== null)
        el.remove();
}
//# sourceMappingURL=teleport.js.map