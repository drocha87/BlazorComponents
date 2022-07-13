var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
export function initializeWindowResizeObserver(dotNetReference) {
    window.addEventListener("resize", (_) => __awaiter(this, void 0, void 0, function* () {
        yield dotNetReference.invokeMethodAsync("WindowResized");
    }));
}
export function calculateTopOffset(sourceRect, targetRect, flipToFit, margin) {
    const { height } = sourceRect;
    const { top, bottom } = targetRect;
    let offset = top - height - margin;
    if (offset < 0 && flipToFit) {
        offset = bottom + margin;
    }
    return offset;
}
export function calculateLeftOffset(sourceRect, targetRect, flipToFit, margin) {
    const { width } = sourceRect;
    const { left, width: tWidth } = targetRect;
    const remainingWidth = window.innerWidth - left - width;
    let offset = left;
    if (remainingWidth <= 0 && flipToFit) {
        offset = left - Math.abs(width - tWidth);
    }
    return offset;
}
var Direction;
(function (Direction) {
    Direction[Direction["Top"] = 0] = "Top";
    Direction[Direction["Right"] = 1] = "Right";
    Direction[Direction["Left"] = 2] = "Left";
    Direction[Direction["Bottom"] = 3] = "Bottom";
})(Direction || (Direction = {}));
function canPlaceOnTop(sourceRect, targetRect, margin) {
    const position = { top: 0, left: 0, needFlip: false };
    const { height } = sourceRect;
    const { top, left } = targetRect;
    position.top = top - height - margin;
    position.left = left;
    if (position.top < 0) {
        position.needFlip = true;
    }
    return position;
}
function canPlaceOnRight(sourceRect, targetRect, margin) {
    const position = { top: 0, left: 0, needFlip: false };
    const { innerWidth } = window;
    const { height, width } = sourceRect;
    const { top: tTop, right: tRight, height: tHeight } = targetRect;
    const center = Math.abs(height - tHeight) / 2;
    position.top = tTop - center;
    position.left = tRight + margin;
    if (position.left + width + margin > innerWidth) {
        position.needFlip = true;
    }
    return position;
}
function canPlaceOnLeft(sourceRect, targetRect, margin) {
    const position = { top: 0, left: 0, needFlip: false };
    const { height, width } = sourceRect;
    const { top: tTop, left: tLeft, height: tHeight } = targetRect;
    const center = Math.abs(height - tHeight) / 2;
    position.top = tTop - center;
    position.left = tLeft - width - margin;
    if (position.left < 0) {
        position.needFlip = true;
    }
    return position;
}
function canPlaceOnBottom(sourceRect, targetRect, margin) {
    const position = { top: 0, left: 0, needFlip: false };
    const { innerHeight } = window;
    const { height } = sourceRect;
    const { bottom, left } = targetRect;
    position.top = bottom + margin;
    position.left = left;
    if (position.top + height > innerHeight) {
        position.needFlip = true;
    }
    return position;
}
export function updatePosition(source, target, direction, flipToFit, margin) {
    const sourceRect = source.getBoundingClientRect();
    const targetRect = target.getBoundingClientRect();
    let top = 0;
    let left = 0;
    switch (direction) {
        case Direction.Top:
            {
                let position = canPlaceOnTop(sourceRect, targetRect, margin);
                if (position.needFlip) {
                    let attemptBottom = canPlaceOnBottom(sourceRect, targetRect, margin);
                    if (!attemptBottom.needFlip) {
                        position = attemptBottom;
                    }
                }
                top = position.top;
                left = position.left;
            }
            break;
        case Direction.Right:
            {
                let position = canPlaceOnRight(sourceRect, targetRect, margin);
                if (position.needFlip && flipToFit) {
                    let attemptLeft = canPlaceOnLeft(sourceRect, targetRect, margin);
                    if (!attemptLeft.needFlip) {
                        position = attemptLeft;
                    }
                }
                top = position.top;
                left = position.left;
            }
            break;
        case Direction.Left:
            {
                let position = canPlaceOnLeft(sourceRect, targetRect, margin);
                if (position.needFlip && flipToFit) {
                    let attemptRight = canPlaceOnRight(sourceRect, targetRect, margin);
                    if (!attemptRight.needFlip) {
                        position = attemptRight;
                    }
                }
                top = position.top;
                left = position.left;
            }
            break;
        case Direction.Bottom:
            {
                let position = canPlaceOnBottom(sourceRect, targetRect, margin);
                if (position.needFlip) {
                    let attemptTop = canPlaceOnTop(sourceRect, targetRect, margin);
                    if (!attemptTop.needFlip) {
                        position = attemptTop;
                    }
                }
                top = position.top;
                left = position.left;
            }
            break;
        default:
            throw new Error("unknown popover direction");
    }
    source.style.top = `${top}px`;
    source.style.left = `${left}px`;
}
//# sourceMappingURL=popover.js.map