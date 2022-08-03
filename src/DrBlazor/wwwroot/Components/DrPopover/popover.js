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
var Direction;
(function (Direction) {
    Direction[Direction["Top"] = 0] = "Top";
    Direction[Direction["Right"] = 1] = "Right";
    Direction[Direction["Left"] = 2] = "Left";
    Direction[Direction["Bottom"] = 3] = "Bottom";
})(Direction || (Direction = {}));
function canPlaceOnTop(elRect, targetRect, margin, target) {
    const position = { top: 0, left: 0, needFlip: false };
    const { height, width } = elRect;
    const { top, left, width: tWidth } = targetRect;
    position.top = top - height - margin;
    position.left = Math.abs(left - (width - tWidth) / 2);
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
    const { height, width } = sourceRect;
    const { bottom, left, width: tWidth } = targetRect;
    position.top = bottom + margin;
    position.left = Math.abs(left - (width - tWidth) / 2);
    if (position.top + height > innerHeight) {
        position.needFlip = true;
    }
    return position;
}
export function updatePosition(el, target, direction, flipToFit, margin) {
    const elRect = el.getBoundingClientRect();
    const targetRect = target.getBoundingClientRect();
    let top = 0;
    let left = 0;
    let placement = "top";
    switch (direction) {
        case Direction.Top:
            {
                let position = canPlaceOnTop(elRect, targetRect, margin, target);
                if (position.needFlip) {
                    let attemptBottom = canPlaceOnBottom(elRect, targetRect, margin);
                    if (!attemptBottom.needFlip) {
                        position = attemptBottom;
                        placement = "bottom";
                    }
                }
                top = position.top;
                left = position.left;
            }
            break;
        case Direction.Right:
            {
                let position = canPlaceOnRight(elRect, targetRect, margin);
                placement = "right";
                if (position.needFlip && flipToFit) {
                    let attemptLeft = canPlaceOnLeft(elRect, targetRect, margin);
                    if (!attemptLeft.needFlip) {
                        position = attemptLeft;
                        placement = "left";
                    }
                }
                top = position.top;
                left = position.left;
            }
            break;
        case Direction.Left:
            {
                let position = canPlaceOnLeft(elRect, targetRect, margin);
                placement = "left";
                if (position.needFlip && flipToFit) {
                    let attemptRight = canPlaceOnRight(elRect, targetRect, margin);
                    if (!attemptRight.needFlip) {
                        position = attemptRight;
                        placement = "right";
                    }
                }
                top = position.top;
                left = position.left;
            }
            break;
        case Direction.Bottom:
            {
                let position = canPlaceOnBottom(elRect, targetRect, margin);
                placement = "bottom";
                if (position.needFlip) {
                    let attemptTop = canPlaceOnTop(elRect, targetRect, margin, target);
                    if (!attemptTop.needFlip) {
                        position = attemptTop;
                        placement = "top";
                    }
                }
                top = position.top;
                left = position.left;
            }
            break;
        default:
            throw new Error("unknown popover direction");
    }
    el.style.top = `${top}px`;
    el.style.left = `${left}px`;
    el.setAttribute("data-dr-popover-placement", placement);
}
//# sourceMappingURL=popover.js.map