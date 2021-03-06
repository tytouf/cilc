
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

namespace LLVM {

public enum VerifierFailureAction {
    AbortProcessAction = 0,
    PrintMessageAction = 1,
    ReturnStatusAction = 2,
};

public abstract class Analysis {

    public static void VerifyModule(Module mod, VerifierFailureAction action, out string message)
    {
        LLVM.VerifyModule(mod, (uint) action, out message);
    }

}

}

