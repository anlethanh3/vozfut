import { useEffect, useState } from 'react';
import Form from 'react-bootstrap/Form'
import FloatingLabel from 'react-bootstrap/FloatingLabel'
import { Modal, Button, Alert, Spinner } from 'react-bootstrap';
import { LoginRequestProps, ProfileProps, onCloseError, onCloseSuccess, loginAsync, selectState } from '../slices/profileSlice';
import { useAppSelector, useAppDispatch } from '../app/hooks';
import { unwrapResult } from '@reduxjs/toolkit';


export default function Login(props: { show: boolean, onSubmit: (profile: ProfileProps) => void, onClose: () => void }) {
    const { show, onSubmit, onClose } = props
    const state = useAppSelector(selectState)
    const dispatch = useAppDispatch();
    const [info, setInfo] = useState<LoginRequestProps>({ email: '', password: '', username: '' })
    const [invalid, setInvalid] = useState({ email: false, password: false })
    const onValid = () => {
        var check = { email: true, password: true }
        if (info.email) {
            check = { ...check, email: false }
        }
        if (info.password) {
            check = { ...check, password: false }
        }
        setInvalid({ ...check })
        if (!check.email && !check.password) {
            onAuthenticate(info)
        }
    }

    const onClickClose = () => {
        if (state.status !== 'loading') {
            onClose()
        }
    }

    const onAuthenticate = (login: LoginRequestProps) => {
        dispatch(loginAsync(login))
            .then(unwrapResult)
            .then((profile) => {
                setTimeout(() => {
                    onSubmit(profile)
                }, 1000)
            })
            .catch((error) => {
                console.log(error)
            })
    }

    return (
        <Modal show={show} onHide={onClickClose} backdrop="static" keyboard={false}>
            <Modal.Header closeButton>
                <Modal.Title>Login</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {
                    <Alert show={state.error !== undefined} variant="danger" onClose={() => dispatch(onCloseError())} dismissible>
                        {state.error}
                    </Alert>
                }
                {
                    <Alert show={state.isSuccess} variant="success" onClose={() => dispatch(onCloseSuccess())} dismissible>
                        Login successfully!
                    </Alert>
                }
                <Form>
                    <Form.Group className="mb-3" controlId="addMember.Name">
                        <FloatingLabel controlId="floatingName" label="Name">
                            <Form.Control type="text" required isInvalid={invalid.email}
                                placeholder="Input email" value={info.email}
                                disabled={state.isLoading}
                                onChange={(e) => setInfo({ ...info, email: e.target.value })}
                            />
                            <Form.Control.Feedback type="invalid">Email is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.Password">
                        <FloatingLabel controlId="floatingPassword" label="Password">
                            <Form.Control type="password" required isInvalid={invalid.password} placeholder="Input Password" value={info.password}
                                disabled={state.isLoading}
                                onChange={(e) => setInfo({ ...info, password: e.target.value })} />
                            <Form.Control.Feedback type="invalid">Password is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button disabled={state.isLoading} variant="primary" onClick={onValid}>
                    {
                        state.isLoading && <Spinner animation="border" variant="light" />
                    }
                    {
                        !state.isLoading && `Login`
                    }

                </Button>
            </Modal.Footer>
        </Modal>
    )
}